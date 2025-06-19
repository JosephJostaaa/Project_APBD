using System.ComponentModel.DataAnnotations;

namespace APBD_Project.Dto;

public class EmployeeRegistrationDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Username must be at most 100 characters long.")]
    public string Username { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Password must be at most 100 characters long.")]
    public string Password { get; set; }
}