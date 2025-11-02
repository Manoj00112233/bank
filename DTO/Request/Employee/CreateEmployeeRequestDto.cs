using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Employee
{
    public class CreateEmployeeRequestDto
    {
        [Required(ErrorMessage ="Client ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        [StringLength(100)]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string Phone { get; set; }

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

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, 10000000, ErrorMessage = "Salary must be between ₹1 and ₹1,00,00,000 per month")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Employee code can only contain uppercase letters, numbers, and hyphens")]
        public string? EmployeeCode { get; set; }

        [Required(ErrorMessage = "Joining date is required")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN must be exactly 10 characters")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format (e.g., ABCDE1234F)")]
        public string? PanNumber { get; set; }
        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}
