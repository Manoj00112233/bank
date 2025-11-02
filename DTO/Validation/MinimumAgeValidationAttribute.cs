using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class MinimumAgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeValidationAttribute(int minimumAge = 18)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not DateTime dateOfBirth)
            {
                return new ValidationResult("Invalid date format");
            }

            int age = DateTime.UtcNow.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.UtcNow.AddYears(-age))
            {
                age--;
            }

            if (age < _minimumAge)
            {
                return new ValidationResult($"Minimum age requirement is {_minimumAge} years");
            }

            return ValidationResult.Success;
        }
    }
    }
