using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class PastDateValidationAttribute : ValidationAttribute
    {
        private readonly bool _allowToday;

        public PastDateValidationAttribute(bool allowToday = true)
        {
            _allowToday = allowToday;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if(value is not DateTime date)
            {
                return new ValidationResult("Invalid Date Format");
            }

            DateTime compareDate = _allowToday ? DateTime.UtcNow.Date : DateTime.UtcNow;

            if (date < compareDate)
            {
                return new ValidationResult(_allowToday ? "Date cannot be in the past" : "Date must be at least tomorrow");
            }
            return ValidationResult.Success;
        }
    }
}
