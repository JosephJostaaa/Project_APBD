using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models.RrsDbModels;

[Table("Client")]
public abstract class Client
{
    [Key]
    public int ClientId { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(150)")]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "NVARCHAR(30)")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string Address { get; set; } = string.Empty;
    [Required]
    public bool IsDeleted { get; set; } = false;
    
    public ICollection<Contract> Contracts { get; set; }
}