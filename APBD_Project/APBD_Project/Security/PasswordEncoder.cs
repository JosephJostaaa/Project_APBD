using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APBD_Project.Security;

public class PasswordEncoder : IPasswordEncoder
{
    public bool Match(string rawPassword, string salt, string inputPassword)
    {
        string hashedInputPassword = GetHashedPasswordWithSalt(inputPassword, salt);
        return hashedInputPassword == rawPassword;
    }
    
    public Tuple<string, string> Hash(string password)
    {
        byte[] salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);
        string saltBase64 = Convert.ToBase64String(salt);

        return new Tuple<string, string>(hashedPassword, saltBase64);
    }

    private static string GetHashedPasswordWithSalt(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        return HashPassword(password, saltBytes);
    }

    private static byte[] GenerateSalt(int size = 128 / 8)
    {
        byte[] salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static string HashPassword(string password, byte[] salt)
    {
        byte[] hashedBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        );

        return Convert.ToBase64String(hashedBytes);
    }
}
