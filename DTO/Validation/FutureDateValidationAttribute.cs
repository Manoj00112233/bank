using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class FutureDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not DateTime date)
            {
                return new ValidationResult("Invalid date format");
            }

            if (date > DateTime.UtcNow)
            {
                return new ValidationResult("Date cannot be in the future");
            }

            return ValidationResult.Success;
        }
    }
}
