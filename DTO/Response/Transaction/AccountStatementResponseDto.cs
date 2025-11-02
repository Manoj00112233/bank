namespace Banking_CapStone.DTO.Response.Transaction
{
    public class AccountStatementResponseDto
    {
        public string AccountNumber { get; set; }
        public string ClientName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public int TotalTransactions { get; set; }
        public List<TransactionHistoryResponseDto> Transactions { get; set; }
    }
}
