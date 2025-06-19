using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Exceptions;
using APBD_Project.Models.EmployeeDbModels;
using APBD_Project.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Project.Services;

public class LoginService : ILoginService
{
    private readonly EmployeeContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordEncoder _passwordEncoder;

    public LoginService(EmployeeContext context, IConfiguration configuration, IPasswordEncoder passwordEncoder)
    {
        _context = context;
        _configuration = configuration;
        _passwordEncoder = passwordEncoder;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        string username = loginRequest.Username;
        string password = loginRequest.Password;
        
        var found = await _context.Employees.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (found == null)
        {
            throw new NotFoundException("Invalid username");
        }
        if (!_passwordEncoder.Match(found.Password, found.Salt, password))
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }
        JwtSecurityToken token = GenerateJwtToken(found.Username, found.Role);
        found.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        found.RefreshTokenExp = DateTime.Now.AddDays(1);
        
        await _context.SaveChangesAsync(cancellationToken);
        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = found.RefreshToken
        };
    }

    public async Task<LoginResponse> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        Employee? user = await _context.Employees
            .Where(u => u.RefreshToken == refreshToken)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException("Invalid refresh token.");
        }
        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        JwtSecurityToken jwtToken = GenerateJwtToken(user.Username, user.Role);
        
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            RefreshToken = user.RefreshToken
        };
    }

    private JwtSecurityToken GenerateJwtToken(string username, string role)
    {
        Claim[] userClaims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)

        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        return jwtToken;
    }
}