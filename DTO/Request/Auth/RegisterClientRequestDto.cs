using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Auth
{
    public class RegisterClientRequestDto
    {
        [Required(ErrorMessage = "Client name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Client name must be between 3 and 100 characters")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can only contain letters, numbers, dots, underscores, and hyphens")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Bank ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bank ID")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Date of registration is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfIncorporation { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        [Range(1, 3, ErrorMessage = "Account type must be 1 (Savings), 2 (Current), or 3 (Salary)")]
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "Initial balance is required")]
        [Range(1000, 10000000, ErrorMessage = "Initial balance must be between 1,000 and 10,000,000")]
        public decimal InitialBalance { get; set; }

        [Required(ErrorMessage = "PAN number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN must be exactly 10 characters")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format (e.g., ABCDE1234F)")]
        public string PanNumber { get; set; }

        [StringLength(15, MinimumLength = 15, ErrorMessage = "GST number must be exactly 15 characters")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST format")]
        public string? GstNumber { get; set; }

        [Required(ErrorMessage ="Bank User ID is required ")]
        public int OnboardedByBankUserId { get; set; }
    }
}
