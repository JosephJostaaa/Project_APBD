using APBD_Project.Dto;

namespace APBD_Project.Services;

public interface IContractService
{
    public Task<Response> CreateContractAsync(ContractCreateDto? createContractDto,
        CancellationToken cancellationToken);

    public Task<Response> MakePaymentAsync(int contractId, PaymentDto paymentDto,
        CancellationToken cancellationToken);
}