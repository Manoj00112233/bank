namespace Banking_CapStone.DTO.Response.DashBoard
{
    public class BankUserDashboardResponseDto
    {
        public string BankName { get; set; }
        public int TotalClients { get; set; }
        public int ActiveClients { get; set; }
        public int PendingPayments { get; set; }
        public int PendingSalaryDisbursements { get; set; }

        public int PendingOnboardings { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public decimal TransactionsToday { get; set; }
        public decimal TransactionVolumeToday { get; set; }
        public List<PendingApprovalDto> PendingApprovals { get; set; }
        public List<RecentClientDto> RecentlyOnboardedClients { get; set; }
    }

    public class PendingApprovalDto
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestedAt { get; set; }

    }

    public class RecentClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime OnboardedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
