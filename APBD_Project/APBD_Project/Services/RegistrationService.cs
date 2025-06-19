using APBD_Project.DAL;
using APBD_Project.Dto;
using APBD_Project.Models.EmployeeDbModels;
using APBD_Project.Security;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.Services;

public class RegistrationService : IRegistrationService
{
    private readonly EmployeeContext _context;
    private readonly IPasswordEncoder _passwordEncoder;

    public RegistrationService(EmployeeContext context, IPasswordEncoder passwordEncoder)
    {
        _context = context;
        _passwordEncoder = passwordEncoder;
    }
    
    public async Task<Response> RegisterEmployeeAsync(EmployeeRegistrationDto employeeDto, CancellationToken cancellationToken)
    {
        var hashResult = _passwordEncoder.Hash(employeeDto.Password);

        string passwordHash = hashResult.Item1;
        string salt = hashResult.Item2;
        
        var employee = new Employee
        {
            Username = employeeDto.Username,
            Password = passwordHash,
            Salt = salt,
            Role = "Employee"
        };
        
        _context.Employees.Add(employee);
        
        await _context.SaveChangesAsync(cancellationToken);

        return new Response { Success = true, Message = "Employee registered successfully." };
    }
}