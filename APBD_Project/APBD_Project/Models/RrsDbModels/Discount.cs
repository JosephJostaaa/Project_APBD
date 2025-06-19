using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models;

[Table("Discount")]
public class Discount
{
    [Key]
    public int DiscountId { get; set; }
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    public string ApplicableTo { get; set; } = string.Empty;
    [Required]
    [Column(TypeName = "DECIMAL(2,2)")]
    public decimal DiscountPercentage { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    private ICollection<Contract> contracts { get; set; } = new List<Contract>();
}