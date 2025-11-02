using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Auth
{
    public class RegisterBankUserRequestDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
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

        [Required(ErrorMessage = "Branch is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        public string Branch { get; set; }

        [Required(ErrorMessage = "Referral code is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Referral code must be between 5 and 20 characters")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Referral code must be uppercase letters and numbers only")]
        public string ReferralCode { get; set; }
    }
}
