using System.Text.Json;
using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.Services;

public class RevenueService : IRevenueService
{
    private readonly RrsContext _context;
    private readonly ICurrencyConverter _currencyConverter;

    public RevenueService(RrsContext context, ICurrencyConverter currencyConverter)
    {
        _context = context;
        _currencyConverter = currencyConverter;
    }
    
    public async Task<RevenueResponseDto> GetRevenueAsync(string? currencyCode, int? productId, CancellationToken cancellationToken)
    {
        var totalRevenueQuery =  _context.Payments
            .Where(p => p.Contract.SignDate != null);

        if (productId != null)
        {
            totalRevenueQuery = totalRevenueQuery
                .Where(p => p.Contract.SoftwareVersion.SoftwareId == productId);
        }
        var totalRevenue = await totalRevenueQuery.SumAsync(p => p.Amount, cancellationToken);
        var result =  await _currencyConverter.AdaptPlnToCurrencyAsync(totalRevenue, currencyCode, cancellationToken);
        
        return new RevenueResponseDto{Amount = result, Currency = currencyCode??"PLN"};
    }
    
    public async Task<RevenueResponseDto> GetPredictedRevenueAsync(string? currencyCode, int? productId, CancellationToken cancellationToken)
    {
        var totalPredictedRevenueQuery = _context.Contracts
            .Where(c => true)
            ;
        if (productId != null)
        {
            totalPredictedRevenueQuery = totalPredictedRevenueQuery
                .Where(c => c.SoftwareVersion.SoftwareId == productId);
        }
        var totalPredictedRevenue =  await totalPredictedRevenueQuery.SumAsync(c => c.FinalPrice, cancellationToken);
        
        var result =  await _currencyConverter.AdaptPlnToCurrencyAsync(totalPredictedRevenue, currencyCode, cancellationToken);
        return new RevenueResponseDto{Amount = result, Currency = currencyCode??"PLN"};
    }
    
}