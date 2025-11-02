using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Account
{
    public class UpdateAccountStatusRequestDto
    {
        [Required(ErrorMessage = "Account ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Account ID")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account status is required")]
        [Range(1, 3, ErrorMessage = "Account status must be 1 (Active), 2 (Inactive), or 3 (Closed)")]
        public int AccountStatusId { get; set; }

        [Required(ErrorMessage = "Reason for status change is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 500 characters")]
        public string ReasonForChange { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }
        [Required(ErrorMessage = "Bank User ID is required")]
        public int UpdatedByBankUserId { get; set; }

        [StringLength(50)]
        public string? ApprovalReference { get; set; }
    }
}
