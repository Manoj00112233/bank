using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.DashBoard;

namespace Banking_CapStone.Service
{
    public interface IDashboardService
    {
        Task<ApiResponseDto<SuperAdminDashboardResponseDto>> GetSuperAdminDashboardAsync(int superAdminId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetSystemWideStatisticsAsync();
        Task<ApiResponseDto<IEnumerable<BankPerformanceDto>>> GetTopPerformingBanksAsync(int count = 10);
        Task<ApiResponseDto<IEnumerable<RecentActivityDto>>> GetRecentSystemActivitiesAsync(int count = 20);

        Task<ApiResponseDto<BankUserDashboardResponseDto>> GetBankUserDashboardAsync(int bankUserId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetBankStatisticsAsync(int bankId);
        Task<ApiResponseDto<IEnumerable<PendingApprovalDto>>> GetPendingApprovalsAsync(int bankId);
        Task<ApiResponseDto<IEnumerable<RecentClientDto>>> GetRecentlyOnboardedClientsAsync(int bankId, int count = 10);
        Task<ApiResponseDto<decimal>> GetBankTransactionVolumeAsync(int bankId, DateTime? fromDate = null, DateTime? toDate = null);

        Task<ApiResponseDto<ClientDashboardResponseDto>> GetClientDashboardAsync(int clientId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetClientFinancialSummaryAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<RecentTransactionDto>>> GetRecentTransactionsAsync(int clientId, int count = 10);
        Task<ApiResponseDto<IEnumerable<UpcomingSalaryDto>>> GetUpcomingSalaryDisbursementsAsync(int clientId);
        Task<ApiResponseDto<decimal>> GetMonthlyExpenditureAsync(int clientId, int? month = null, int? year = null);

        Task<ApiResponseDto<Dictionary<string, decimal>>> GetMonthlyTrendsAsync(int entityId, string entityType, int months = 6);
        Task<ApiResponseDto<Dictionary<string, int>>> GetActivityBreakdownAsync(int entityId, string entityType);

        Task<ApiResponseDto<Dictionary<string, object>>> GetComparativeAnalysisAsync(int entityId, string entityType, DateTime fromDate, DateTime toDate);
    }
}
