using System.ComponentModel.DataAnnotations;
using APBD_Project.DAL;

namespace APBD_Project.Dto;

public class PersonCreateDto : ClientCreateDto
{
    [Required]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string LastName { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL must be exactly 11 characters long.")]
    public string Pesel { get; set; }
}