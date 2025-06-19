using System.Text.Json;
using APBD_Project.Exceptions;

namespace APBD_Project.Services;

public class CurrencyConverter : ICurrencyConverter
{
    public async Task<decimal> AdaptPlnToCurrencyAsync(decimal revenue, string? currencyCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(currencyCode) && !string.Equals(currencyCode.Trim(), "PLN", StringComparison.OrdinalIgnoreCase))
        {
            revenue = await ConvertPlnToCurrencyAsync(revenue, currencyCode.Trim().ToUpper(), cancellationToken);
        }

        return revenue;
    }

    private async Task<decimal> ConvertPlnToCurrencyAsync(decimal amount, string targetCurrency, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        string url = $"https://api.frankfurter.app/latest?from=PLN&to={targetCurrency}&amount={amount}";

        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new CurrencyConversionException($"Currency conversion failed with status code {response.StatusCode}.");

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<FrankfurterResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result?.Rates is null || !result.Rates.TryGetValue(targetCurrency, out var convertedAmount))
            throw new CurrencyConversionException("Currency not found in the response.");

        return convertedAmount;
    }

    private class FrankfurterResponse
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}