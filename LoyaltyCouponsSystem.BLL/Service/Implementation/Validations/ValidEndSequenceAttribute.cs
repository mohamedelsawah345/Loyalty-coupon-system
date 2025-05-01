using System.ComponentModel.DataAnnotations;

public class ValidEndSequenceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var endSequenceNumber = (int)value;

        var startSequenceNumber = (int)validationContext.ObjectType
            .GetProperty("StartSequenceNumber")?.GetValue(validationContext.ObjectInstance);

        if (endSequenceNumber <= startSequenceNumber)
        {
            return new ValidationResult("End sequence number must be greater than start sequence number.");
        }

        return ValidationResult.Success;
    }
}
