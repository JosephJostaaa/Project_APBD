using System.ComponentModel.DataAnnotations;

namespace APBD_Project.Dto;

public class LoginRequest
{
    [Required]
    [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
    public string Username { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; set; }
}