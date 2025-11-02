using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Validation
{
    public class IFSCValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string ifsc = value.ToString(); 

            if(ifsc.Length != 11)
            {
                return new ValidationResult("IFSC code must be exactly 11 characters");
            }

            if(!System.Text.RegularExpressions.Regex.IsMatch(ifsc , @"[A-Z]{4}0[A-Z0-9]{6}$"))
            {
                return new ValidationResult("Invalid IFSC format. Must be: 4 letters + 0 + 6 alphanumeric (e.g., HDFC0001234)");
            }
            return ValidationResult.Success;
        }
    }
}
