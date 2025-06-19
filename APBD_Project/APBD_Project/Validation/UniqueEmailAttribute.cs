using APBD_Project.DAL;

namespace APBD_Project.Validation;

using System.ComponentModel.DataAnnotations;

public class UniqueEmailAttribute : ValidationAttribute
{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; 
        }
        
        var rrsContext = (RrsContext?)validationContext.GetService(typeof(RrsContext));
        if (rrsContext == null)
        {
            throw new InvalidOperationException("DbContext is not available");
        }

        string email = value.ToString()!;

        bool emailExists = rrsContext.Clients.Any(c => c.Email == email);

        return emailExists ? new ValidationResult("Email must be unique.") : ValidationResult.Success;
    }
}
