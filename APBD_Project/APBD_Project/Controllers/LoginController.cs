using APBD_Project.Dto;
using APBD_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = APBD_Project.Dto.LoginRequest;

namespace APBD_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _loginService.Login(loginRequest, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
    
    [HttpPost("refresh")]
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest refreshToken, CancellationToken cancellationToken)
    {
        var result = await _loginService.RefreshToken(refreshToken.RefreshToken, cancellationToken);
        return Ok(result);
    }

}