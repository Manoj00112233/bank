using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(BankingDbContext context) : base(context) { }

        public async Task<AuditLog> LogActionAsync(string actionType, int userId, string performedBy, string? details = null)
        {
            var auditLog = new AuditLog
            {
                ActionType = actionType,
                PerformedBy = performedBy,
                Timestamp = DateTime.UtcNow,
                Details = details,
                UserId = userId
            };

            await AddAsync(auditLog);
            return auditLog;
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserIdAsync(int userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByActionTypeAsync(string actionType)
        {
            return await _context.AuditLogs
                .Where(a => a.ActionType == actionType)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.AuditLogs
                .Where(a => a.Timestamp >= fromDate && a.Timestamp <= toDate)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<(IEnumerable<AuditLog> Logs, int TotalCount)> GetAuditLogsPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(a => a.ActionType.Contains(filter.SearchTerm) ||
                                             a.PerformedBy.Contains(filter.SearchTerm));
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp <= filter.ToDate.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (logs, totalCount);
        }

        public async Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 50)
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalAuditLogsCountAsync(int? userId = null)
        {
            if (userId.HasValue)
            {
                return await _context.AuditLogs.CountAsync(a => a.UserId == userId.Value);
            }
            return await _context.AuditLogs.CountAsync();
        }

        public async Task<Dictionary<string, object>> GetAuditStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(a => a.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(a => a.Timestamp <= toDate.Value);

            var stats = new Dictionary<string, object>
            {
                ["TotalLogs"] = await query.CountAsync(),
                ["UniqueUsers"] = await query.Select(a => a.UserId).Distinct().CountAsync(),
                ["ActionTypeBreakdown"] = await query
                    .GroupBy(a => a.ActionType)
                    .Select(g => new { ActionType = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.ActionType, x => (object)x.Count)
            };

            return stats;
        }
    }
}