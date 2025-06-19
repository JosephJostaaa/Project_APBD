using System.Security.Cryptography;

namespace APBD_Project.Security;

public static class SecurityHelpers
{
    public static string GenerateRefreshToken(int size = 32)
    {
        var randomBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }
}
