using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<ApiResponseDto<AuditLog>> LogActionAsync(string actionType, int userId, string performedBy, string? details = null)
        {
            try
            {
                var auditLog = await _auditLogRepository.LogActionAsync(actionType, userId, performedBy, details);
                return ApiResponseDto<AuditLog>.SuccessResponse(auditLog);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AuditLog>.ErrorResponse($"Error logging action: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AuditLog>> LogLoginAsync(int userId, string username, bool success)
        {
            var actionType = success ? "LOGIN_SUCCESS" : "LOGIN_FAILED";
            var details = success ? "User logged in successfully" : "Login attempt failed";
            return await LogActionAsync(actionType, userId, username, details);
        }

        public async Task<ApiResponseDto<AuditLog>> LogLogoutAsync(int userId, string username)
        {
            return await LogActionAsync("LOGOUT", userId, username, "User logged out");
        }

        public async Task<ApiResponseDto<AuditLog>> LogCreateAsync(string entityName, int entityId, int userId, string username)
        {
            return await LogActionAsync("CREATE", userId, username, $"Created {entityName} with ID {entityId}");
        }

        public async Task<ApiResponseDto<AuditLog>> LogUpdateAsync(string entityName, int entityId, int userId, string username, string? details = null)
        {
            var detailsText = details ?? $"Updated {entityName} with ID {entityId}";
            return await LogActionAsync("UPDATE", userId, username, detailsText);
        }

        public async Task<ApiResponseDto<AuditLog>> LogDeleteAsync(string entityName, int entityId, int userId, string username)
        {
            return await LogActionAsync("DELETE", userId, username, $"Deleted {entityName} with ID {entityId}");
        }

        public async Task<ApiResponseDto<AuditLog>> LogPaymentApprovalAsync(int paymentId, int bankUserId, string username, bool approved)
        {
            var actionType = approved ? "PAYMENT_APPROVED" : "PAYMENT_REJECTED";
            var details = $"Payment {paymentId} was {(approved ? "approved" : "rejected")}";
            return await LogActionAsync(actionType, bankUserId, username, details);
        }

        public async Task<ApiResponseDto<AuditLog>> LogSalaryDisbursementAsync(int disbursementId, int bankUserId, string username, bool approved)
        {
            var actionType = approved ? "SALARY_APPROVED" : "SALARY_REJECTED";
            var details = $"Salary Disbursement {disbursementId} was {(approved ? "approved" : "rejected")}";
            return await LogActionAsync(actionType, bankUserId, username, details);
        }

        public async Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByUserIdAsync(int userId)
        {
            try
            {
                var logs = await _auditLogRepository.GetAuditLogsByUserIdAsync(userId);
                return ApiResponseDto<IEnumerable<AuditLog>>.SuccessResponse(logs);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AuditLog>>.ErrorResponse($"Error retrieving audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByActionTypeAsync(string actionType)
        {
            try
            {
                var logs = await _auditLogRepository.GetAuditLogsByActionTypeAsync(actionType);
                return ApiResponseDto<IEnumerable<AuditLog>>.SuccessResponse(logs);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AuditLog>>.ErrorResponse($"Error retrieving audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<AuditLog>>> GetAuditLogsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var logs = await _auditLogRepository.GetAuditLogsByDateRangeAsync(fromDate, toDate);
                return ApiResponseDto<IEnumerable<AuditLog>>.SuccessResponse(logs);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AuditLog>>.ErrorResponse($"Error retrieving audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<AuditLog>>> GetAuditLogsPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (logs, totalCount) = await _auditLogRepository.GetAuditLogsPaginatedAsync(pagination, filter);

                var paginatedResponse = new PaginatedResponseDto<AuditLog>(
                    logs.ToList(), totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<AuditLog>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<AuditLog>>.ErrorResponse($"Error retrieving audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<AuditLog>>> GetRecentAuditLogsAsync(int count = 50)
        {
            try
            {
                var logs = await _auditLogRepository.GetRecentAuditLogsAsync(count);
                return ApiResponseDto<IEnumerable<AuditLog>>.SuccessResponse(logs);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AuditLog>>.ErrorResponse($"Error retrieving audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetTotalAuditLogsCountAsync(int? userId = null)
        {
            try
            {
                var count = await _auditLogRepository.GetTotalAuditLogsCountAsync(userId);
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting audit logs: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetAuditStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var stats = await _auditLogRepository.GetAuditStatisticsAsync(fromDate, toDate);
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }
    }
}