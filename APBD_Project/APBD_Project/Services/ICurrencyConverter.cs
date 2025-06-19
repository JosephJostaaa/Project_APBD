namespace APBD_Project.Services;

public interface ICurrencyConverter
{
    public Task<decimal> AdaptPlnToCurrencyAsync(decimal revenue, string? currencyCode,
        CancellationToken cancellationToken);
}