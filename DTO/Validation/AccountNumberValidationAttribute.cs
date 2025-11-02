using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class AccountNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string accountNumber = value.ToString();

            if (!System.Text.RegularExpressions.Regex.IsMatch(accountNumber, @"^BPA\d{8}[A-Z0-9]{6}$"))
            {
                return new ValidationResult("Account number must follow format: BPA + 8 digits + 6 alphanumeric (e.g., BPA12345678ABC123)");
            }
            return ValidationResult.Success;
        }
    }
}
