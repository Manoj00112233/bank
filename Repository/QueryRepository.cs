using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class QueryRepository : BaseRepository<Query>, IQueryRepository
    {
        public QueryRepository(BankingDbContext context) : base(context) { }

        public async Task<Query?> GetQueryWithDetailsAsync(int queryId)
        {
            return await _context.Queries
                .Include(q => q.RespondedByUser)
                .FirstOrDefaultAsync(q => q.QueryId == queryId);
        }

        public async Task<IEnumerable<Query>> GetAllQueriesAsync()
        {
            return await _context.Queries
                .Include(q => q.RespondedByUser)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Query>> GetPendingQueriesAsync()
        {
            return await _context.Queries
                .Where(q => !q.IsResolved)
                .OrderBy(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Query>> GetResolvedQueriesAsync()
        {
            return await _context.Queries
                .Where(q => q.IsResolved)
                .Include(q => q.RespondedByUser)
                .OrderByDescending(q => q.RespondedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Query>> GetQueriesByPriorityAsync(string priority)
        {
            return await _context.Queries
                .Where(q => q.Priority == priority)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Query>> GetQueriesByCategoryAsync(string category)
        {
            return await _context.Queries
                .Where(q => q.Category == category)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Query> Queries, int TotalCount)> GetQueriesPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Queries
                .Include(q => q.RespondedByUser)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(q =>
                        q.Subject.Contains(filter.SearchTerm) ||
                        q.Message.Contains(filter.SearchTerm) ||
                        q.Name.Contains(filter.SearchTerm) ||
                        q.Email.Contains(filter.SearchTerm));
                }

                if (!string.IsNullOrEmpty(filter.Category))
                {
                    query = query.Where(q => q.Category == filter.Category);
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(q => q.IsResolved != filter.IsActive.Value);
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(q => q.CreatedAt >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(q => q.CreatedAt <= filter.ToDate.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var queries = await query
                .OrderByDescending(q => q.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (queries, totalCount);
        }

        public async Task<bool> RespondToQueryAsync(int queryId, string response, int respondedByUserId)
        {
            var query = await GetByIdAsync(queryId);
            if (query == null) return false;

            query.Response = response;
            query.RespondedBy = respondedByUserId;
            query.RespondedAt = DateTime.UtcNow;
            query.IsResolved = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsResolvedAsync(int queryId)
        {
            var query = await GetByIdAsync(queryId);
            if (query == null) return false;

            query.IsResolved = true;
            if (!query.RespondedAt.HasValue)
                query.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsUnresolvedAsync(int queryId)
        {
            var query = await GetByIdAsync(queryId);
            if (query == null) return false;

            query.IsResolved = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetPendingQueriesCountAsync()
        {
            return await _context.Queries.CountAsync(q => !q.IsResolved);
        }

        public async Task<int> GetResolvedQueriesCountAsync()
        {
            return await _context.Queries.CountAsync(q => q.IsResolved);
        }

        public async Task<IEnumerable<Query>> GetQueriesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Queries
                .Where(q => q.CreatedAt >= fromDate && q.CreatedAt <= toDate)
                .Include(q => q.RespondedByUser)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<Dictionary<string, object>> GetQueryStatisticsAsync()
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalQueries"] = await _context.Queries.CountAsync(),
                ["PendingQueries"] = await GetPendingQueriesCountAsync(),
                ["ResolvedQueries"] = await GetResolvedQueriesCountAsync(),
                ["HighPriorityQueries"] = await _context.Queries.CountAsync(q => q.Priority == "High" && !q.IsResolved),
                ["AverageResponseTime"] = await CalculateAverageResponseTimeAsync(),
                ["QueriesThisMonth"] = await _context.Queries
                    .CountAsync(q => q.CreatedAt.Year == DateTime.UtcNow.Year &&
                                    q.CreatedAt.Month == DateTime.UtcNow.Month)
            };

            return stats;
        }

        private async Task<double> CalculateAverageResponseTimeAsync()
        {
            var resolvedQueries = await _context.Queries
                .Where(q => q.IsResolved && q.RespondedAt.HasValue)
                .ToListAsync();

            if (!resolvedQueries.Any()) return 0;

            var totalHours = resolvedQueries
                .Sum(q => (q.RespondedAt!.Value - q.CreatedAt).TotalHours);

            return totalHours / resolvedQueries.Count();
        }
    }
}
