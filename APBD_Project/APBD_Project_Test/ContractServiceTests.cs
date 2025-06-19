using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models;
using APBD_Project.Models.RrsDbModels;
using APBD_Project.Services;

namespace APBD_Project_Test;

public class ContractServiceTests
{
    [Fact]
    public async Task CreateContractAsync_ReturnsError_WhenDtoIsNull()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();
        var service = new ContractService(context);

        // Act
        var result = await service.CreateContractAsync(null, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid contract data", result.Message);
    }

    [Fact]
    public async Task CreateContractAsync_ThrowsNotFound_WhenVersionNotFound()
    {
        var context = TestHelpers.GetMockContext();
        var dto = new ContractCreateDto { ClientId = 1, VersionId = 999, EndDate = DateTime.Now.AddYears(1) };

        var service = new ContractService(context);

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateContractAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateContractAsync_ThrowsNotFound_WhenClientNotFound()
    {
        var context = TestHelpers.GetMockContext();
        context.SoftwareVersions.Add(new SoftwareVersion { SoftwareVersionId = 1, BasePrice = 1000 });
        context.SaveChanges();

        var dto = new ContractCreateDto { ClientId = 1, VersionId = 1, EndDate = DateTime.Now.AddYears(1) };

        var service = new ContractService(context);

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateContractAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateContractAsync_ReturnsError_WhenActiveContractExists()
    {
        var context = TestHelpers.GetMockContext();

        context.SoftwareVersions.Add(new SoftwareVersion { SoftwareVersionId = 1, BasePrice = 1000 });
        var client = new Company { ClientId = 1, IsDeleted = false, Contracts = new List<Contract>() };
        client.Contracts.Add(new Contract
        {
            SoftwareVersionId = 1,
            EndDate = DateTime.Now.AddDays(30) // active
        });
        context.Clients.Add(client);
        context.SaveChanges();

        var dto = new ContractCreateDto { ClientId = 1, VersionId = 1, EndDate = DateTime.Now.AddYears(1) };
        var service = new ContractService(context);

        var result = await service.CreateContractAsync(dto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Client already has a contract for this software version", result.Message);
    }

    [Fact]
    public async Task CreateContractAsync_CreatesContract_WithoutDiscountOrPriorContracts()
    {
        var context = TestHelpers.GetMockContext();

        context.SoftwareVersions.Add(new SoftwareVersion { SoftwareVersionId = 1, BasePrice = 1000 });
        var client = new Company { ClientId = 1, IsDeleted = false, Contracts = new List<Contract>() };
        context.Clients.Add(client);
        context.SaveChanges();

        var dto = new ContractCreateDto
            { ClientId = 1, VersionId = 1, EndDate = DateTime.Now.AddYears(1), SupportYears = 2 };
        var service = new ContractService(context);

        var result = await service.CreateContractAsync(dto, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("Contract created successfully", result.Message);
        Assert.Single(context.Contracts);
        var contract = context.Contracts.First();
        Assert.Equal(1000 + 2 * 1000, contract.FinalPrice); // No discount, no multiplier
    }

    [Fact]
    public async Task CreateContractAsync_AppliesBestDiscountAndClientMultiplier()
    {
        
        var context = TestHelpers.GetMockContext();

        
        var software = new Software
        {
            SoftwareId = 1,
            Name = "Test Software",
            Description = "Test Description",
            Category = "Test"
        };
        context.Softwares.Add(software);
        
        var softwareVersion = new SoftwareVersion
        {
            SoftwareVersionId = 1,
            BasePrice = 2000,
            SoftwareId = 1,
            Software = software,
            Version = "v1.0",
            ReleaseDate = DateTime.Now
        };
        context.SoftwareVersions.Add(softwareVersion);
        
        context.Discounts.AddRange(
            new Discount
            {
                DiscountId = 1,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1),
                DiscountPercentage = 0.10m,
                ApplicableTo = "contract"
            },
            new Discount
            {
                DiscountId = 2,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1),
                DiscountPercentage = 0.20m,
                ApplicableTo = "contract"
            }
        );
        
        var client = new Company
        {
            ClientId = 1,
            IsDeleted = false,
            Contracts = new List<Contract>()
        };

        var oldContract = new Contract
        {
            ClientId = 1,
            SoftwareVersionId = 2,
            EndDate = DateTime.Now.AddDays(-1)
        };
        client.Contracts.Add(oldContract);
        context.Clients.Add(client);

        context.Contracts.Add(oldContract);
        context.SaveChanges();

        var dto = new ContractCreateDto
        {
            ClientId = 1,
            VersionId = 1,
            EndDate = DateTime.Now.AddYears(1),
            SupportYears = 1
        };

        var service = new ContractService(context);
        
        var result = await service.CreateContractAsync(dto, CancellationToken.None);

        
        Assert.True(result.Success);
        var contract = context.Contracts.First(c => c.SoftwareVersionId == 1);

        decimal basePlusSupport = 2000 + 1000;
        decimal discountApplied = basePlusSupport * 0.8m;
        decimal withClientMultiplier = discountApplied * 1.05m;
        Assert.Equal(withClientMultiplier, contract.FinalPrice);
    }
}