using System.ComponentModel.DataAnnotations;

namespace APBD_Project.DAL;

public class CompanyCreateDto : ClientCreateDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Company name must be at most 100 characters long.")]
    public string CompanyName { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 10)]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "KRS must be exactly 10 digits.")]
    public string Krs { get; set; }
}