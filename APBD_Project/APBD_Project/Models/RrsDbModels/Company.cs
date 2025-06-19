using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models.RrsDbModels;

public class Company : Client
{
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string CompanyName { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "NVARCHAR(11)")]
    public string Krs { get; set; } = string.Empty;
    
}