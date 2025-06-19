using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models;

[Table("Contract")]
public class Contract
{
    [Key]
    public int ContractId { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int SoftwareVersionId { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal FinalPrice { get; set; }
    public int? UpdateYears { get; set; }
    public DateTime? SignDate { get; set; }
    public int DiscountId { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
    
    [ForeignKey(nameof(DiscountId))]
    public Discount? Discount { get; set; }

    [ForeignKey(nameof(SoftwareVersionId))]
    public SoftwareVersion SoftwareVersion { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
}