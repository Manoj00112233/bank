using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task<AuditLog> LogActionAsync(string actionType, int userId, string performedBy, string? details = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsByUserIdAsync(int userId);
        Task<IEnumerable<AuditLog>> GetAuditLogsByActionTypeAsync(string actionType);
        Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<(IEnumerable<AuditLog> Logs, int TotalCount)> GetAuditLogsPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 50);
        Task<int> GetTotalAuditLogsCountAsync(int? userId = null);
        Task<Dictionary<string, object>> GetAuditStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    }
}
