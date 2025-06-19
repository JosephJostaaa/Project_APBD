namespace APBD_Project.Security;

public interface IPasswordEncoder
{
    public bool Match(string rawPassword, string salt, string inputPassword);
    public Tuple<string, string> Hash(string password);
}