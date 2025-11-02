using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Payment
{
    public class CreatePaymentRequestDto
    {
        [Required(ErrorMessage = "Client ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Beneficiary ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Beneficiary ID")]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 10000000, ErrorMessage = "Amount must be between ₹1 and ₹1,00,00,000")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment purpose is required")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Payment purpose must be between 10 and 200 characters")]
        public string PaymentPurpose { get; set; }

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ScheduledDate { get; set; }

        public bool IsUrgent { get; set; } = false;

        public string? InvoiceNumber { get; set; }

        public List<int> SupportingDocumentIds  { get; set; }


    }
}
