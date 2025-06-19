using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models.RrsDbModels;
using APBD_Project.Services;


namespace APBD_Project_Test;

public class ClientServiceTests
{
    [Fact]
    public async Task AddCompanyAsync_ReturnsSuccess_WhenValidDto()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();
        var service = new ClientService(context);

        var dto = new CompanyCreateDto
        {
            Email = "company@test.com",
            Address = "123 Test Street",
            PhoneNumber = "1234567890",
            CompanyName = "Test Corp",
            Krs = "123456"
        };

        // Act
        var result = await service.AddCompanyAsync(dto, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Company created successfully.", result.Message);
        Assert.Single(context.Companies);
    }

    [Fact]
    public async Task UpdatePersonAsync_ReturnsFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();
        
        var person1 = new Person
        {
            ClientId = 1,
            Email = "existing@example.com",
            IsDeleted = false
        };
        var person2 = new Person
        {
            ClientId = 2,
            Email = "original@example.com",
            IsDeleted = false
        };
        context.Persons.AddRange(person1, person2);
        await context.SaveChangesAsync();

        var service = new ClientService(context);

        var updateDto = new PersonUpdateDto
        {
            Email = "existing@example.com"
        };

        // Act
        var result = await service.UpdatePersonAsync(2, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Email already exists.", result.Message);
    }
    

    [Fact]
    public async Task AddPersonAsync_NullDto_ReturnsFailureResponse()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();
        var service = new ClientService(context);

        PersonCreateDto? dto = null;

        // Act
        var result = await service.AddPersonAsync(dto, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid person data.", result.Message);
        Assert.Empty(context.Persons);
    }

    [Fact]
    public async Task AddPersonAsync_ValidDto_AddsPersonAndReturnsSuccess()
    {
        // Arrange
        var context = TestHelpers.GetMockContext();
        var service = new ClientService(context);

        var dto = new PersonCreateDto
        {
            Email = "test@example.com",
            Address = "123 Test St",
            PhoneNumber = "123456789",
            FirstName = "John",
            LastName = "Doe",
            Pesel = "12345678901"
        };

        // Act
        var result = await service.AddPersonAsync(dto, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Person created successfully.", result.Message);
        var person = Assert.Single(context.Persons);
        Assert.Equal(dto.Email, person.Email);
        Assert.Equal(dto.Address, person.Address);
        Assert.Equal(dto.PhoneNumber, person.PhoneNumber);
        Assert.Equal(dto.FirstName, person.FirstName);
        Assert.Equal(dto.LastName, person.LastName);
        Assert.Equal(dto.Pesel, person.Pesel);
    }
    
    [Fact]
public async Task UpdateCompanyAsync_NullDto_ReturnsFailureResponse()
{
    // Arrange
    var context = TestHelpers.GetMockContext();
    var service = new ClientService(context);

    // Act
    var result = await service.UpdateCompanyAsync(1, null, CancellationToken.None);

    // Assert
    Assert.False(result.Success);
    Assert.Equal("Invalid company data.", result.Message);
}

[Fact]
public async Task UpdateCompanyAsync_CompanyNotFound_ThrowsNotFoundException()
{
    // Arrange
    var context = TestHelpers.GetMockContext();
    var service = new ClientService(context);

    // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() =>
        service.UpdateCompanyAsync(999, new CompanyUpdateDto { Email = "newemail@test.com" }, CancellationToken.None));
}

[Fact]
public async Task UpdateCompanyAsync_CompanyIsDeleted_ThrowsNotFoundException()
{
    // Arrange
    var context = TestHelpers.GetMockContext();

    var company = new Company
    {
        ClientId = 1,
        Email = "oldemail@test.com",
        IsDeleted = true,
        Address = "Old Address",
        PhoneNumber = "1234567890",
        CompanyName = "Old Company"
    };
    context.Companies.Add(company);
    await context.SaveChangesAsync();

    var service = new ClientService(context);

    // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() =>
        service.UpdateCompanyAsync(company.ClientId, new CompanyUpdateDto { Email = "newemail@test.com" }, CancellationToken.None));
}

[Fact]
public async Task UpdateCompanyAsync_EmailExists_ReturnsFailureResponse()
{
    // Arrange
    var context = TestHelpers.GetMockContext();

    var existingCompany = new Company
    {
        ClientId = 1,
        Email = "existing@test.com",
        IsDeleted = false
    };
    var companyToUpdate = new Company
    {
        ClientId = 2,
        Email = "original@test.com",
        IsDeleted = false
    };
    context.Companies.AddRange(existingCompany, companyToUpdate);
    await context.SaveChangesAsync();

    var service = new ClientService(context);

    var updateDto = new CompanyUpdateDto { Email = "existing@test.com" };

    // Act
    var result = await service.UpdateCompanyAsync(companyToUpdate.ClientId, updateDto, CancellationToken.None);

    // Assert
    Assert.False(result.Success);
    Assert.Equal("Email already exists.", result.Message);
}

[Fact]
public async Task UpdateCompanyAsync_ValidUpdate_UpdatesFieldsAndReturnsSuccess()
{
    // Arrange
    var context = TestHelpers.GetMockContext();

    var company = new Company
    {
        ClientId = 1,
        Email = "oldemail@test.com",
        IsDeleted = false,
        Address = "Old Address",
        PhoneNumber = "1234567890",
        CompanyName = "Old Company"
    };
    context.Companies.Add(company);
    await context.SaveChangesAsync();

    var service = new ClientService(context);

    var updateDto = new CompanyUpdateDto
    {
        Email = "newemail@test.com",
        Address = "New Address",
        PhoneNumber = "0987654321",
        CompanyName = "New Company"
    };

    // Act
    var result = await service.UpdateCompanyAsync(company.ClientId, updateDto, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    Assert.Equal("Company updated successfully.", result.Message);

    var updatedCompany = await context.Companies.FindAsync(company.ClientId);
    Assert.Equal(updateDto.Email, updatedCompany.Email);
    Assert.Equal(updateDto.Address, updatedCompany.Address);
    Assert.Equal(updateDto.PhoneNumber, updatedCompany.PhoneNumber);
    Assert.Equal(updateDto.CompanyName, updatedCompany.CompanyName);
}

[Fact]
public async Task DeleteClientAsync_ClientExistsAndNotDeleted_ReturnsOneAndMarksDeleted()
{
    // Arrange
    var context = TestHelpers.GetMockContext();

    var client = new Person
    {
        ClientId = 1,
        Email = "test@test.com",
        IsDeleted = false,
        FirstName = "John",
        LastName = "Doe",
        Pesel = "12345678901",
        PhoneNumber = "123456789",
        Address = "Test Address"
    };
    context.Persons.Add(client);
    await context.SaveChangesAsync();

    var service = new ClientService(context);

    // Act
    await service.DeleteClientAsync(client.ClientId, CancellationToken.None);

    // Assert

    var deletedClient = await context.Clients.FindAsync(client.ClientId);
    Assert.True(deletedClient.IsDeleted);
}


}
