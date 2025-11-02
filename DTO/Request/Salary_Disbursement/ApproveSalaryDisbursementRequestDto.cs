using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Salary_Disbursement
{
    public class ApproveSalaryDisbursementRequestDto
    {
        [Required(ErrorMessage = "Salary Disbursement ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Salary Disbursement ID")]
        public int SalaryDisbursementId { get; set; }

        [Required(ErrorMessage = "Bank User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bank User ID")]
        public int BankUserId { get; set; }

        [Required(ErrorMessage = "Approval remarks are required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Approval remarks must be between 10 and 500 characters")]
        public string ApprovalRemarks { get; set; }

        [StringLength(100)]
        public string? ApprovalReference { get; set; }

        public bool VerifiedEmployeeCount { get; set; } = true;

        public bool VerifiedTotalAmount { get; set; } = true;

        public bool VerifiedClientBalance { get; set; } = true;

        public string? BatchProcessingReference { get; set; }

        public bool ProcessInBatch { get; set; } = true;    
    }
}
