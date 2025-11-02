using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IQueryRepository : IBaseRepository<Query>
    {
        Task<Query?> GetQueryWithDetailsAsync(int queryId);
        Task<IEnumerable<Query>> GetAllQueriesAsync();
        Task<IEnumerable<Query>> GetPendingQueriesAsync();
        Task<IEnumerable<Query>> GetResolvedQueriesAsync();
        Task<IEnumerable<Query>> GetQueriesByPriorityAsync(string priority);
        Task<IEnumerable<Query>> GetQueriesByCategoryAsync(string category);
        Task<(IEnumerable<Query> Queries, int TotalCount)> GetQueriesPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<bool> RespondToQueryAsync(int queryId, string response, int respondedByUserId);
        Task<bool> MarkAsResolvedAsync(int queryId);
        Task<bool> MarkAsUnresolvedAsync(int queryId);
        Task<int> GetPendingQueriesCountAsync();
        Task<int> GetResolvedQueriesCountAsync();
        Task<IEnumerable<Query>> GetQueriesByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<Dictionary<string, object>> GetQueryStatisticsAsync();
        
    }
}
