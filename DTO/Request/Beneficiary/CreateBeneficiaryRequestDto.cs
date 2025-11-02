using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Beneficiary
{
    public class CreateBeneficiaryRequestDto
    {
        [Required(ErrorMessage = "Client ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Beneficiary name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Beneficiary name must be between 3 and 100 characters")]

        public string BeneficiaryName { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Account number must be between 9 and 20 characters")]
        [RegularExpression(@"^[0-9A-Z]+$", ErrorMessage = "Account number can only contain numbers and uppercase letters")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bank name must be between 3 and 100 characters")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "IFSC code is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "IFSC code must be exactly 11 characters")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format (e.g., HDFC0001234)")]
        public string IFSC { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }

        [StringLength(200, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 200 characters")]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? BeneficiaryType { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }    

        public bool ActivateImmediately { get; set; }
    }
}
