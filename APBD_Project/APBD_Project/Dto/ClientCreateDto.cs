using System.ComponentModel.DataAnnotations;
using APBD_Project.Validation;

namespace APBD_Project.DAL;

public class ClientCreateDto
{
    [Required]
    [UniqueEmail]
    [EmailAddress]
    [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
    public string Email { get; set; }
    [Required]
    [Phone]
    [StringLength(30, ErrorMessage = "Phone number cannot be longer than 30 characters.")]
    public string PhoneNumber { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
    public string Address { get; set; }
}