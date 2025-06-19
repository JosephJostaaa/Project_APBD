using System.ComponentModel.DataAnnotations;

namespace APBD_Project.Dto;

public class PersonUpdateDto : ClientUpdateDto
{
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    public string? FirstName { get; set; }
    
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string? LastName { get; set; }
}