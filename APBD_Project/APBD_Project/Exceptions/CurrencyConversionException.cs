namespace APBD_Project.Exceptions;

public class CurrencyConversionException : Exception
{
    public CurrencyConversionException(string? message) : base(message)
    {
    }
}