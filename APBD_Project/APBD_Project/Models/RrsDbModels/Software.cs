using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models;

[Table("Software")]
public class Software
{
    [Key]
    public int SoftwareId { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "NVARCHAR(500)")]
    public string Description { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string Category { get; set; } = string.Empty;
    ICollection<SoftwareVersion> SoftwareVersions { get; set; } = new List<SoftwareVersion>();
}