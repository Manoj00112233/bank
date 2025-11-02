using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Auth
{
    public class RegisterSuperAdminRequestDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can only contain letters, numbers, dots, underscores, and hyphens")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [StringLength(100,MinimumLength =8,ErrorMessage ="Password must be at least 8 character")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm password is required")]
        [Compare("Password",ErrorMessage ="Password and confirmation password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage ="Invalid Phone Number Format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }
    }
}
