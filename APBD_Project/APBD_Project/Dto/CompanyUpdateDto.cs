using System.ComponentModel.DataAnnotations;

namespace APBD_Project.Dto;

public class CompanyUpdateDto : ClientUpdateDto
{
    [StringLength(100, ErrorMessage = "Company name must be at most 100 characters long.")]
    public string? CompanyName { get; set; }
}