using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Salary_Disbursement
{
    public class RejectSalaryDisbursementRequestDto
    {
        [Required(ErrorMessage = "Salary Disbursement ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Salary Disbursement ID")]
        public int SalaryDisbursementId { get; set; }

        [Required(ErrorMessage = "Bank User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bank User ID")]
        public int BankUserId { get; set; }

        [Required(ErrorMessage = "Rejection reason is required")]
        [StringLength(500, MinimumLength = 20, ErrorMessage = "Rejection reason must be between 20 and 500 characters")]
        public string RejectionReason { get; set; }

        [Required(ErrorMessage = "Rejection category is required")]
        [StringLength(100)]
        public string RejectionCategory { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public bool NotifyClient { get; set; } = true;

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        public bool AllowResubmission { get; set; } = true; 
    }
}
