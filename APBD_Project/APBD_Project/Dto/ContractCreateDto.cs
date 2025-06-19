using System.ComponentModel.DataAnnotations;
using APBD_Project.Validation;

namespace APBD_Project.Dto;

public class ContractCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "ClientId must be a positive integer.")]
    [Required(ErrorMessage = "ClientId is required.")]
    public int ClientId { get; set; }
    [Required(ErrorMessage = "VersionId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ContractId must be a positive integer.")]
    public int VersionId { get; set; }
    [Required(ErrorMessage = "EndDate is required.")]
    [FutureDate]
    public DateTime EndDate { get; set; }
    [Range(1, 3, ErrorMessage = "SupportYears must be between 1 and 3.")]
    public int? SupportYears { get; set; }
}