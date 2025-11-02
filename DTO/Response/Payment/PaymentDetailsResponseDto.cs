namespace Banking_CapStone.DTO.Response.Payment
{
    public class PaymentDetailsResponseDto
    {
        public int PaymentId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAccountNumber { get; set; }
        public int? BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string BeneficiaryIFSC { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentPurpose { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedByBankUserName { get; set; }
        public string? ApprovalRemarks { get; set; }
        public string? RejectionReason { get; set; }
        public List<TransactionSummaryDto> Transactions { get; set; }
    }

    public class TransactionSummaryDto
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
