using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Security;
using APBD_Project.Services;

namespace APBD_Project_Test;

using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class RegistrationServiceTests
{

    [Fact]
    public async Task RegisterEmployeeAsync_ValidDto_RegistersEmployeeSuccessfully()
    {
        // Arrange
        var context = TestHelpers.GetContext();
        var passwordEncoderMock = new Mock<IPasswordEncoder>();
        passwordEncoderMock.Setup(pe => pe.Hash(It.IsAny<string>()))
            .Returns(new Tuple<string, string>("hashedPassword", "saltValue"));

        var service = new RegistrationService(context, passwordEncoderMock.Object);

        var dto = new EmployeeRegistrationDto
        {
            Username = "newuser",
            Password = "password123"
        };

        // Act
        var result = await service.RegisterEmployeeAsync(dto, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Employee registered successfully.", result.Message);

        var savedEmployee = context.Employees.SingleOrDefault(e => e.Username == dto.Username);
        Assert.NotNull(savedEmployee);
        Assert.Equal("hashedPassword", savedEmployee.Password);
        Assert.Equal("saltValue", savedEmployee.Salt);
        Assert.Equal("Employee", savedEmployee.Role);
    }

    [Fact]
    public async Task RegisterEmployeeAsync_CallsPasswordEncoderWithCorrectPassword()
    {
        // Arrange
        var context = TestHelpers.GetContext();
        var passwordEncoderMock = new Mock<IPasswordEncoder>();
        string passwordPassed = null;

        passwordEncoderMock.Setup(pe => pe.Hash(It.IsAny<string>()))
            .Callback<string>(pass => passwordPassed = pass)
            .Returns(new Tuple<string, string>("hashed", "salt"));

        var service = new RegistrationService(context, passwordEncoderMock.Object);

        var dto = new EmployeeRegistrationDto
        {
            Username = "user1",
            Password = "mypassword"
        };

        // Act
        await service.RegisterEmployeeAsync(dto, CancellationToken.None);

        // Assert
        Assert.Equal("mypassword", passwordPassed);
    }

    [Fact]
    public void EmployeeRegistrationDto_ValidationFailsForEmptyUsername()
    {
        var dto = new EmployeeRegistrationDto
        {
            Username = "",  // empty username
            Password = "somepassword"
        };

        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, ctx, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(dto.Username)));
    }

    [Fact]
    public void EmployeeRegistrationDto_ValidationFailsForEmptyPassword()
    {
        var dto = new EmployeeRegistrationDto
        {
            Username = "user",
            Password = ""  // empty password
        };

        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, ctx, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(dto.Password)));
    }
}
