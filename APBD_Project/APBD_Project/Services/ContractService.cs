using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.Services;

public class ContractService : IContractService
{
    private readonly RrsContext _context;

    public ContractService(RrsContext context)
    {
        _context = context;
    }

    public async Task<Response> CreateContractAsync(ContractCreateDto? createContractDto,
        CancellationToken cancellationToken)
    {
        if (createContractDto == null)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid contract data"
            };
        }

        var version = await _context.SoftwareVersions
            .FirstOrDefaultAsync(s => s.SoftwareVersionId == createContractDto.VersionId, cancellationToken);
        if (version == null)
            throw new NotFoundException("Version not found");

        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.ClientId == createContractDto.ClientId && c.IsDeleted == false, cancellationToken);
        
        if (client == null)
            throw new NotFoundException("Client not found");

        var now = DateTime.Now;
        
        var bestDiscount = await _context.Discounts
            .Where(d => d.StartDate <= now && d.EndDate >= now && d.ApplicableTo.Equals("contract"))
            .OrderByDescending(d => d.DiscountPercentage)
            .FirstOrDefaultAsync(cancellationToken);

        var supportYears = createContractDto.SupportYears ?? 0;
        
        decimal finalPrice = (version.BasePrice + supportYears * 1000) *
                             (1 - (bestDiscount?.DiscountPercentage ?? 0));

        if (client.Contracts.Count > 0)
        {
            finalPrice *= 1.05m;
        }

        if (client.Contracts
            .Any(c => c.SoftwareVersionId == createContractDto.VersionId &&
                      c.EndDate >= DateTime.Now))
        {
            return new Response
            {
                Success = false,
                Message = "Client already has a contract for this software version"
            };
        }

        var contract = new Contract
        {
            ClientId = createContractDto.ClientId,
            SoftwareVersionId = createContractDto.VersionId,
            StartDate = DateTime.Now,
            EndDate = createContractDto.EndDate,
            Discount = bestDiscount,
            FinalPrice = finalPrice,
            UpdateYears = 1 + supportYears
        };

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response { Success = true, Message = "Contract created successfully" };
    }

    public async Task<Response> MakePaymentAsync(int contractId, PaymentDto paymentDto,
        CancellationToken cancellationToken)
    {
        decimal amount = paymentDto.Amount;
        
        var contract = await _context.Contracts
            .Include(c => c.Client)
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);

        if (contract == null)
            throw new NotFoundException("Contract not found");

        if (contract.SignDate != null)
        {
            return new Response
            {
                Success = false,
                Message = "Contract has already been signed"
            };
        }

        if (amount <= 0)
        {
            return new Response
            {
                Success = false,
                Message = "Invalid payment amount"
            };
        }

        if (contract.EndDate < DateTime.Now)
        {
            return new Response
            {
                Success = false,
                Message = "The deadline for making payments for this contract has passed"
            };
        }

        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var totalPaid = contract.Payments.Sum(p => p.Amount) + amount;
            if (contract.FinalPrice == totalPaid)
            {
                contract.SignDate = DateTime.Now;
            }
            else if (contract.FinalPrice < totalPaid)
            {
                return new Response
                {
                    Success = false,
                    Message = "Payment exceeds the final price of the contract"
                };
            }

            var payment = new Payment
            {
                Contract = contract,
                Amount = amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new Response { Success = true, Message = "Payment made successfully" };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new Response
            {
                Success = false,
                Message = $"An error occurred while processing the payment: {ex.Message}"
            };
        }
    }
}