using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Service
{
    public interface IAuditLogService
    {
        Task<ApiResponseDto<AuditLog>> LogActionAsync(string actionType, int userId, string performedBy, string? details = null);
        Task<ApiResponseDto<AuditLog>> LogLoginAsync(int userId, string username, bool success);
        Task<ApiResponseDto<AuditLog>> LogLogoutAsync(int userId, string username);
        Task<ApiResponseDto<AuditLog>> LogCreateAsync(string entityName, int entityId, int userId, string username);
        Task<ApiResponseDto<AuditLog>> LogUpdateAsync(string entityName, int entityId, int userId, string username, string? details = null);
        Task<ApiResponseDto<AuditLog>> LogDeleteAsync(string entityName, int entityId, int userId, string username);
        Task<ApiResponseDto<AuditLog>> LogPaymentApprovalAsync(int paymentId, int bankUserId, string username, bool approved);
        Task<ApiResponseDto<AuditLog>> LogSalaryDisbursementAsync(int disbursementId, int bankUserId, string username, bool approved);
        Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByUserIdAsync(int userId);
        Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByActionTypeAsync(string actionType);
        Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<PaginatedResponseDto<AuditLog>>> GetAuditLogsPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<IEnumerable<AuditLog>>> GetRecentAuditLogsAsync(int count = 50);
        Task<ApiResponseDto<int>> GetTotalAuditLogsCountAsync(int? userId = null);
        Task<ApiResponseDto<Dictionary<string, object>>> GetAuditStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);

    }
}
