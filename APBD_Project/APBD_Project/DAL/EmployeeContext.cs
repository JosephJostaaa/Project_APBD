using APBD_Project.Models.EmployeeDbModels;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.DAL;

public class EmployeeContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected EmployeeContext()
    {
    }

    public EmployeeContext(DbContextOptions options) : base(options)
    {
    }
}