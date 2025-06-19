using System.ComponentModel.DataAnnotations;

namespace APBD_Project.Dto;

public class PaymentDto
{
    [Required]
    [Range(0.01, int.MaxValue, ErrorMessage = "Amount must be a positive value.")]
    public decimal Amount { get; set; }
}