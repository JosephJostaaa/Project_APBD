using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models.EmployeeDbModels;
using APBD_Project.Security;
using APBD_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace APBD_Project_Test;

public class LoginServiceTests
{
    

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var salt = "salt123";
        var hashedPassword = "hashedPass";
        context.Employees.Add(new Employee
        {
            Username = "user",
            Password = hashedPassword,
            Salt = salt,
            Role = "Admin"
        });
        await context.SaveChangesAsync();

        encoder.Setup(e => e.Match(hashedPassword, salt, "password")).Returns(true);

        var service = new LoginService(context, config, encoder.Object);
        var result = await service.Login(new LoginRequest { Username = "user", Password = "password" }, default);

        // Assert
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task Login_InvalidUsername_ThrowsNotFound()
    {
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var service = new LoginService(context, config, encoder.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.Login(new LoginRequest { Username = "wrong", Password = "password" }, default));
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsUnauthorized()
    {
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var salt = "salt";
        context.Employees.Add(new Employee
        {
            Username = "user",
            Password = "hashed",
            Salt = salt,
            Role = "Admin"
        });
        await context.SaveChangesAsync();

        encoder.Setup(e => e.Match("hashed", salt, "wrongpassword")).Returns(false);

        var service = new LoginService(context, config, encoder.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.Login(new LoginRequest { Username = "user", Password = "wrongpassword" }, default));
    }

    [Fact]
    public async Task RefreshToken_ValidToken_ReturnsNewJwt()
    {
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var oldRefresh = "oldtoken";
        var emp = new Employee
        {
            Username = "user",
            Password = "pass",
            Salt = "salt",
            Role = "Admin",
            RefreshToken = oldRefresh,
            RefreshTokenExp = DateTime.Now.AddMinutes(5)
        };
        context.Employees.Add(emp);
        await context.SaveChangesAsync();

        var service = new LoginService(context, config, encoder.Object);
        var result = await service.RefreshToken(oldRefresh, default);

        Assert.NotNull(result.Token);
        Assert.NotEqual(oldRefresh, result.RefreshToken);
    }

    [Fact]
    public async Task RefreshToken_InvalidToken_ThrowsNotFound()
    {
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var service = new LoginService(context, config, encoder.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.RefreshToken("wrongtoken", default));
    }

    [Fact]
    public async Task RefreshToken_ExpiredToken_ThrowsSecurityException()
    {
        var context = TestHelpers.GetContext();
        var encoder = new Mock<IPasswordEncoder>();
        var config = TestHelpers.GetConfiguration();

        var expiredToken = "expired";
        context.Employees.Add(new Employee
        {
            Username = "user",
            Password = "pass",
            Salt = "salt",
            Role = "Admin",
            RefreshToken = expiredToken,
            RefreshTokenExp = DateTime.Now.AddMinutes(-1)
        });
        await context.SaveChangesAsync();

        var service = new LoginService(context, config, encoder.Object);

        await Assert.ThrowsAsync<SecurityTokenException>(() =>
            service.RefreshToken(expiredToken, default));
    }
}