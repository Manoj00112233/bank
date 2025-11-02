using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Employee
{
    public class UpdateEmployeeRequestDto
    {
        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Employee ID")]
        public int EmployeeId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string? EmployeeName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }

        [StringLength(20, MinimumLength = 9, ErrorMessage = "Account number must be between 9 and 20 characters")]
        [RegularExpression(@"^[0-9A-Z]+$", ErrorMessage = "Account number can only contain numbers and uppercase letters")]
        public string? AccountNumber { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bank name must be between 3 and 100 characters")]
        public string? BankName { get; set; }


        [StringLength(11, MinimumLength = 11, ErrorMessage = "IFSC code must be exactly 11 characters")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format (e.g., HDFC0001234)")]
        public string? IFSC { get; set; }

        [Range(1, 10000000, ErrorMessage = "Salary must be between ₹1 and ₹1,00,00,000")]
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 500 characters")]
        public string? ReasonForUpdate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastWorkingDate { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Required(ErrorMessage = "Client ID is required")]
        public int ClientId { get; set; }
    }

}
