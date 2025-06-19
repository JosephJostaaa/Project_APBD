using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models.RrsDbModels;

[Table("software_version")]
public class SoftwareVersion
{
    [Key]
    public int SoftwareVersionId { get; set; }
    [Required]
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal BasePrice { get; set; }
    [Required]
    public int SoftwareId { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string Version { get; set; } = string.Empty;
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; }
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}