using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Salary_Disbursement
{
    public class CreateSalaryDisbursementRequestDto
    {
        [Required(ErrorMessage = "Client ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Please specify if disbursing to all employees")]
        public bool AllEmployees { get; set; } = true;

        // Comma-separated employee IDs if AllEmployees = false
        [StringLength(500)]
        public string? SelectedEmployeeIds { get; set; }

        [Required(ErrorMessage ="Salary Month is required")]
        [Range(1,12 , ErrorMessage ="Month must be between 1 to 12")]
        public int SalaryMonth {  get; set; }

        [Required(ErrorMessage = "Salary year is required")]
        [Range(2020, 2035, ErrorMessage = "Year must be between 2020 and 2035")]
        public int SalaryYear { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ScheduledDisbursementDate { get; set; }

        public string? Remarks { get; set; }

        public bool IsBonus { get; set; } = false;

        [Range(0, 10000, ErrorMessage = "Bonus amount must be between ₹0 and ₹10,00,0")]
        [DataType(DataType.Currency)]
        public decimal? BonusAmount { get; set; }

        [StringLength(200)]
        public string? PayrollReference { get; set; }

        public bool DeductTDS { get; set; } = false;

        public bool ProcessImmediately { get; set; } = false;   
    }
}
