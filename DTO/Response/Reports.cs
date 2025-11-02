namespace Banking_CapStone.DTO.Response.Reports
{
    public class TransactionReportDto
    {
        public int TransactionId { get; set; }
        public string ClientName { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class PaymentReportDto
    {
        public int PaymentId { get; set; }
        public string ClientName { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class AuditLogReportDto
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
