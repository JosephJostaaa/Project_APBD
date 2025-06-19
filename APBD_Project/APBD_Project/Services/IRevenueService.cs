using APBD_Project.Dto;

namespace APBD_Project.Services;

public interface IRevenueService
{
    public Task<RevenueResponseDto> GetRevenueAsync(string? countryCode, int? productId, CancellationToken cancellationToken);
    public Task<RevenueResponseDto> GetPredictedRevenueAsync(string? countryCode, int? productId, CancellationToken cancellationToken);
}