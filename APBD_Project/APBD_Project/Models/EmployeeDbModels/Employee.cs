using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models.EmployeeDbModels;

[Table("Employee")]
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string Username { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(200)")]
    public string Password { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string Salt { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string Role { get; set; } = "Employee";
    [Column(TypeName = "NVARCHAR(150)")]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
}