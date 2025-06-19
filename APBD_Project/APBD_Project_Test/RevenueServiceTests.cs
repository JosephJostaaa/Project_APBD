using APBD_Project.Models.RrsDbModels;
using APBD_Project.Services;
using Moq;

namespace APBD_Project_Test;

public class RevenueServiceTests
{
    [Fact]
    public async Task GetRevenueAsync_NoProductId_ReturnsTotalRevenueInPLN()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();

        var software = new Software { SoftwareId = 1 };
        var version = new SoftwareVersion { SoftwareVersionId = 1, Software = software, SoftwareId = 1 };
        var contract1 = new Contract { ContractId = 1, SoftwareVersion = version, SignDate = System.DateTime.Now };
        var contract2 = new Contract { ContractId = 2, SoftwareVersion = version, SignDate = System.DateTime.Now };

        context.Contracts.AddRange(contract1, contract2);

        context.Payments.AddRange(
            new Payment { PaymentId = 1, Amount = 100, Contract = contract1 },
            new Payment { PaymentId = 2, Amount = 200, Contract = contract2 },
            new Payment { PaymentId = 3, Amount = 300, Contract = new Contract {ContractId = 3, SignDate = null } }
        );

        await context.SaveChangesAsync();

        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock.Setup(c => c.AdaptPlnToCurrencyAsync(It.IsAny<decimal>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((decimal amount, string? currency, CancellationToken ct) => amount);

        var service = new RevenueService(context, currencyConverterMock.Object);

        // Act
        var result = await service.GetRevenueAsync(null, null, CancellationToken.None);

        // Assert
        Assert.Equal(300, result.Amount); // 100 + 200
        Assert.Equal("PLN", result.Currency);
    }

    [Fact]
    public async Task GetRevenueAsync_WithProductId_FiltersPaymentsAndConvertsCurrency()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();

        var software1 = new Software { SoftwareId = 1 };
        var software2 = new Software { SoftwareId = 2 };
        var version1 = new SoftwareVersion { SoftwareVersionId = 1, Software = software1, SoftwareId = 1 };
        var version2 = new SoftwareVersion { SoftwareVersionId = 2, Software = software2, SoftwareId = 2 };
        var contract1 = new Contract { ContractId = 1, SoftwareVersion = version1, SignDate = System.DateTime.Now };
        var contract2 = new Contract { ContractId = 2, SoftwareVersion = version2, SignDate = System.DateTime.Now };

        context.Contracts.AddRange(contract1, contract2);

        context.Payments.AddRange(
            new Payment { PaymentId = 1, Amount = 100, Contract = contract1 },
            new Payment { PaymentId = 2, Amount = 200, Contract = contract2 }
        );

        await context.SaveChangesAsync();

        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock.Setup(c => c.AdaptPlnToCurrencyAsync(100, "USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(120m); // Simulate conversion

        var service = new RevenueService(context, currencyConverterMock.Object);

        // Act
        var result = await service.GetRevenueAsync("USD", 1, CancellationToken.None);

        // Assert
        Assert.Equal(120m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public async Task GetPredictedRevenueAsync_NoProductId_ReturnsSumOfAllContractsInPLN()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();

        var software = new Software { SoftwareId = 1 };
        var version = new SoftwareVersion { SoftwareVersionId = 1, Software = software, SoftwareId = 1 };

        context.Contracts.AddRange(
            new Contract { ContractId = 1, SoftwareVersion = version, FinalPrice = 500 },
            new Contract { ContractId = 2, SoftwareVersion = version, FinalPrice = 1500 }
        );

        await context.SaveChangesAsync();

        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock.Setup(c => c.AdaptPlnToCurrencyAsync(It.IsAny<decimal>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((decimal amount, string? currency, CancellationToken ct) => amount);

        var service = new RevenueService(context, currencyConverterMock.Object);

        // Act
        var result = await service.GetPredictedRevenueAsync(null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2000, result.Amount);
        Assert.Equal("PLN", result.Currency);
    }

    [Fact]
    public async Task GetPredictedRevenueAsync_WithProductId_FiltersContractsAndConvertsCurrency()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();

        var software1 = new Software { SoftwareId = 1 };
        var software2 = new Software { SoftwareId = 2 };
        var version1 = new SoftwareVersion { SoftwareVersionId = 1, Software = software1, SoftwareId = 1 };
        var version2 = new SoftwareVersion { SoftwareVersionId = 2, Software = software2, SoftwareId = 2 };

        context.Contracts.AddRange(
            new Contract { ContractId = 1, SoftwareVersion = version1, FinalPrice = 1000 },
            new Contract { ContractId = 2, SoftwareVersion = version2, FinalPrice = 2000 }
        );

        await context.SaveChangesAsync();

        var currencyConverterMock = new Mock<ICurrencyConverter>();
        currencyConverterMock.Setup(c => c.AdaptPlnToCurrencyAsync(1000, "EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(900m);

        var service = new RevenueService(context, currencyConverterMock.Object);

        // Act
        var result = await service.GetPredictedRevenueAsync("EUR", 1, CancellationToken.None);

        // Assert
        Assert.Equal(900m, result.Amount);
        Assert.Equal("EUR", result.Currency);
    }
}