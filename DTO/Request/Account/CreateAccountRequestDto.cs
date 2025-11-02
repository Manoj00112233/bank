using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Account
{
    public class CreateAccountRequestDto
    {
        [Required(ErrorMessage ="Client ID is required")]
        [Range(1,int.MaxValue , ErrorMessage ="Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage ="Bank ID is required ")]
        [Range(1,int.MaxValue , ErrorMessage ="Invalid bank id")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        [Range(1, 3, ErrorMessage = "Account type must be 1 (Savings), 2 (Current), or 3 (Salary)")]
        public int AccountTypeId { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^BPA\d{8}[A-Z0-9]{6}$",
            ErrorMessage = "Account number must follow format: BPA + 8 digits + 6 alphanumeric (e.g., BPA12345678ABC123)")]
        public string? AccountNumber { get; set; }

        [Required(ErrorMessage = "Initial balance is required")]
        [Range(0, 1000000000, ErrorMessage = "Initial balance must be between ₹0 and ₹100,00,00,000")]
        public decimal InitialBalance { get; set; }

        public bool ActivateImmediately { get; set; } = false;

        [StringLength(200)]
        public string? AccountPurpose { get; set; }

        [Required(ErrorMessage = "Bank User ID is required")]
        public int CreatedByBankUserId { get; set; }

    }
}
