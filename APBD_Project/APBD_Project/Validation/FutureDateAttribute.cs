namespace APBD_Project.Validation;

using System;
using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date > DateTime.Now;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be a future date.";
    }
}
