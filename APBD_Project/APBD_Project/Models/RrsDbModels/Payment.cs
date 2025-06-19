using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Project.Models;

[Table("Payment")]
public class Payment
{
    [Key]
    public int PaymentId { get; set; }
    [Required]
    public int ContractId { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }
    [Required]
    [Column(TypeName = "DECIMAL(8,2)")]
    public decimal Amount { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }
}