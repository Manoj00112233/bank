using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        private readonly bool _validatePrefix;

        public PhoneNumberValidationAttribute(bool validatePrefix = true)
        {
            _validatePrefix = validatePrefix;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string phone = value.ToString()!;

            
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9]{10}$"))
            {
                return new ValidationResult("Phone number must be exactly 10 digits");
            }

            if (_validatePrefix && !phone.StartsWith("6") && !phone.StartsWith("7") &&
                !phone.StartsWith("8") && !phone.StartsWith("9"))
            {
                return new ValidationResult("Phone number must start with 6, 7, 8, or 9");
            }

            return ValidationResult.Success;
        }
    }
}