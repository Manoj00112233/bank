namespace Banking_CapStone.DTO.Response.Transaction
{
    public class TransactionResponseDto
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public int? PaymentId { get; set; }
        public int? SalaryDisbursementId { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
