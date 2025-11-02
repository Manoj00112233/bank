using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class PanCardValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string pan = value.ToString()!.ToUpper();

            if (pan.Length != 10)
            {
                return new ValidationResult("PAN must be exactly 10 characters");
            }

            
            if (!System.Text.RegularExpressions.Regex.IsMatch(pan, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
            {
                return new ValidationResult("Invalid PAN format. Must be: 5 letters + 4 digits + 1 letter (e.g., ABCDE1234F)");
            }

            char fourthChar = pan[3];
            if (!IsValidEntityType(fourthChar))
            {
                return new ValidationResult($"Invalid PAN entity type character: {fourthChar}");
            }

            return ValidationResult.Success;
        }
        private bool IsValidEntityType(char entityType)
        {
            // Valid entity types in PAN
            return "CPHABFGLJT".Contains(entityType);
        }
    }
}
