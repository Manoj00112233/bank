using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Query
{
    public class CreateQueryRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 200 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Message must be between 20 and 1000 characters")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50)]
        public string Category { get; set; } // Technical, Account, Payment, General, SalaryDisbursement

        [Required(ErrorMessage = "Priority is required")]
        [StringLength(20)]
        public string Priority { get; set; } // Low, Medium, High, Critical
    }
}
