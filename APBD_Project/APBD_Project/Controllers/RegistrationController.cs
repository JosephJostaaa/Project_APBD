using APBD_Project.Dto;
using APBD_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Project.Controllers;

[ApiController]
[Route("api/")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto registrationDto, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var result = await _registrationService.RegisterEmployeeAsync(registrationDto, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Created();
    }
}