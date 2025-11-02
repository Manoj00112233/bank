using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Payment
{
    public class ApprovePaymentRequestDto
    {
        [Required(ErrorMessage ="Payment ID is required")]
        [Range(1, int.MaxValue,ErrorMessage ="Invalid Payment ID")]
        public int paymentId { get; set; }

        [Required(ErrorMessage = "Bank User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bank User ID")]
        public int BankUserId { get; set; }

        [Required(ErrorMessage = "Approval remarks are required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Approval remarks must be between 10 and 500 characters")]
        public string ApprovalRemarks { get; set; }


        public string? TransactionReference { get; set; }

        public bool VerifiedDocuments { get; set; } = true;

        public bool VerifiedBeneficiary { get; set; } = true;

        public bool VerifiedClientBalance { get; set; } = true;
    }
}
