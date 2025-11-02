namespace Banking_CapStone.DTO.Response.Transaction
{
    public class TransactionHistoryResponseDto
    {
        public int TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
