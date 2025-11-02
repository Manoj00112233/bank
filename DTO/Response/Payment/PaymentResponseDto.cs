namespace Banking_CapStone.DTO.Response.Payment
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int? BeneficiaryId { get; set; }
        public string? BeneficiaryName { get;  set; }

        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string? PaymentPurpose { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedByBankUserId { get; set; }
        public string? ApprovedByBankUserName { get; set; }
        public string? Remarks { get; set; }
    }
}
