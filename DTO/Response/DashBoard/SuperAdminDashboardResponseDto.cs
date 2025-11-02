namespace Banking_CapStone.DTO.Response.DashBoard
{
    public class SuperAdminDashboardResponseDto
    {
        public int TotalBanks { get; set; }
        public int ActiveBank {  get; set; }
        public int TotalClients { get; set; }
        public int TotalBankUsers { get; set; }

        public decimal TotalSystemBalance { get; set; }
        public int TotalTransactionsToday { get; set; }

        public decimal TotalTransactionVolumeToday { get; set; }

        public int PendingQueries { get; set; }

        public List<BankPerformanceDto> TopBanks { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; }


    }

    public class BankPerformanceDto
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int TotalClients { get; set; }
        public decimal TotalTransactionVolume { get; set; }
    }

    public class RecentActivityDto 
    {
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
