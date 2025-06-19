using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models;

public class Person : Client
{
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "NVARCHAR(11)")]
    public string Pesel { get; set; } = string.Empty;
    
}