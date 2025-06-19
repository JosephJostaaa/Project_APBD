using APBD_Project.Dto;

namespace APBD_Project.Services;

public interface IRegistrationService
{
    public Task<Response> RegisterEmployeeAsync(EmployeeRegistrationDto employeeDto,
        CancellationToken cancellationToken);
}