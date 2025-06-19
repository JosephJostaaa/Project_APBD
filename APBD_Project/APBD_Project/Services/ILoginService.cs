using APBD_Project.Dto;

namespace APBD_Project.Services;

public interface ILoginService
{
    public Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken);
    public Task<LoginResponse> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}