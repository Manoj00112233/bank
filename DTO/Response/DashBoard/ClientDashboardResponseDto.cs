using Banking_CapStone.DTO.Response.Client;

namespace Banking_CapStone.DTO.Response.DashBoard
{
    public class ClientDashboardResponseDto
    {
        public string ClientName { get; set; }
        public decimal TotalBalance { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalBeneficiaries { get; set; }
        public int PendingPayments { get; set; }
        public int PendingSalaryDisbursements { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public decimal TransactionsThisMonth { get; set; }
        public decimal TotalSpentThisMonth { get; set; }

        public List<AccountSummaryDto> Accounts { get; set; }
        public List<RecentTransactionDto> RecentTransactions { get; set; }
        public List<UpcomingSalaryDto> UpcomingSalaries { get; set; }
    }

    public class RecentTransactionDto
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date {  get; set; }
    }

    public class UpcomingSalaryDto
    {
        public int SalaryDisbursementId { get; set; }
        public int EmployeeCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
