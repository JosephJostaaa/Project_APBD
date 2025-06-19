using APBD_Project.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APBD_Project_Test;

public class TestHelpers
{
    public static RrsContext GetMockContext()
    {
        var options = new DbContextOptionsBuilder<RrsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RrsContext(options);
    }
    
    public static EmployeeContext GetContext()
    {
        var options = new DbContextOptionsBuilder<EmployeeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new EmployeeContext(options);
    }
    public static IConfiguration GetConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:SecretKey", "supersecretkey123456789sdfsdsdsdf0!" }
            }!)
            .Build();
        return config;
    }
}