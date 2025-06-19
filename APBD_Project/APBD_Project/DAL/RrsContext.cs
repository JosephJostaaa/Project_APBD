using APBD_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Project.DAL;

public class RrsContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Client> Clients { get; set; }
    
    public RrsContext(DbContextOptions<RrsContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Person>().ToTable("Person");
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<Client>().ToTable("Client");

        modelBuilder.Entity<Software>()
            .HasData(
                new Software { SoftwareId = 1, Name = "Intellij IDEA", Description = "Software for Java developers", Category = "Education" },
                new Software { SoftwareId = 2, Name = "Visual Studio", Description = "Software for .NET developers", Category = "Education" },
                new Software { SoftwareId = 3, Name = "Eclipse", Description = "Software for Java developers", Category = "Education" },
                new Software { SoftwareId = 4, Name = "PyCharm", Description = "Software for Python developers", Category = "Education" },
                new Software { SoftwareId = 5, Name = "WebStorm", Description = "Software for JavaScript developers", Category = "Education" }
                );

        modelBuilder.Entity<SoftwareVersion>()
            .HasData(
                new SoftwareVersion { SoftwareVersionId = 1, BasePrice = 1000, SoftwareId = 1, Version = "2023.1", ReleaseDate = new DateTime(2023, 5, 1) },
                new SoftwareVersion { SoftwareVersionId = 2, BasePrice = 1200, SoftwareId = 2, Version = "17.0.1", ReleaseDate = new DateTime(2023, 3, 10) },
                new SoftwareVersion { SoftwareVersionId = 3, BasePrice = 1200, SoftwareId = 2, Version = "17.0.2", ReleaseDate = new DateTime(2023, 4, 15) },
                new SoftwareVersion { SoftwareVersionId = 4, BasePrice = 800, SoftwareId = 3, Version = "4.25", ReleaseDate = new DateTime(2023, 6, 10) },
                new SoftwareVersion { SoftwareVersionId = 5, BasePrice = 800, SoftwareId = 3, Version = "4.26", ReleaseDate = new DateTime(2023, 6, 30) },
                new SoftwareVersion { SoftwareVersionId = 6, BasePrice = 900, SoftwareId = 4, Version = "2023.2", ReleaseDate = new DateTime(2023, 7, 20) },
                new SoftwareVersion { SoftwareVersionId = 7, BasePrice = 1100, SoftwareId = 5, Version = "2023.1", ReleaseDate = new DateTime(2023, 8, 5) }
            );

        modelBuilder.Entity<Discount>()
            .HasData(
                new Discount { DiscountId = 1, DiscountPercentage = 0.10m, ApplicableTo = "contract", StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 12, 31) },
                new Discount { DiscountId = 2, DiscountPercentage = 0.15m, ApplicableTo = "contract", StartDate = new DateTime(2025, 6, 1), EndDate = new DateTime(2025, 6, 30) },
                new Discount { DiscountId = 3, DiscountPercentage = 0.20m, ApplicableTo = "subscription", StartDate = new DateTime(2025, 5, 1), EndDate = new DateTime(2025, 11, 30) }
            );

        modelBuilder.Entity<Company>()
            .HasData(
                new Company{ ClientId = 1, Email = "company1@gmail.com", PhoneNumber = "123456789", Address = "123 Main St, City, Country", IsDeleted = false, CompanyName = "Google", Krs = "1234567890"},
                new Company{ ClientId = 2, Email = "company2@gmail.com", PhoneNumber = "987654321", Address = "456 Elm St, City, Country", IsDeleted = false, CompanyName = "Microsoft", Krs = "0987654321"}
            );

        modelBuilder.Entity<Person>()
            .HasData(
                new Person{ ClientId = 3, Email = "person3@gmail.com", PhoneNumber = "555555555", Address = "789 Oak St, City, Country", IsDeleted = false, FirstName = "John", LastName = "Doe", Pesel = "12345678901" },
                new Person{ ClientId = 4, Email = "person4@gmail.com", PhoneNumber = "444444444", Address = "321 Pine St, City, Country", IsDeleted = false, FirstName = "Jane", LastName = "Smith", Pesel = "10987654321" }
            );
    }
}