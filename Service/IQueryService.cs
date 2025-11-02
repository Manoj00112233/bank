using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Query;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Query;

namespace Banking_CapStone.Service
{
    public interface IQueryService
    {
        Task<ApiResponseDto<QueryResponseDto>> CreateQueryAsync(CreateQueryRequestDto request);
        Task<ApiResponseDto<QueryResponseDto>> GetQueryByIdAsync(int queryId);
        Task<ApiResponseDto<QueryResponseDto>> GetQueryWithDetailsAsync(int queryId);
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetAllQueriesAsync();
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetPendingQueriesAsync();
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetResolvedQueriesAsync();
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByPriorityAsync(string priority);
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByCategoryAsync(string category);
        Task<ApiResponseDto<PaginatedResponseDto<QueryListResponseDto>>> GetQueriesPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<QueryResponseDto>> RespondToQueryAsync(RespondToQueryRequestDto request);
        Task<ApiResponseDto<bool>> MarkAsResolvedAsync(int queryId);
        Task<ApiResponseDto<bool>> MarkAsUnresolvedAsync(int queryId);
        Task<ApiResponseDto<int>> GetPendingQueriesCountAsync();
        Task<ApiResponseDto<int>> GetResolvedQueriesCountAsync();
        Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<Dictionary<string, object>>> GetQueryStatisticsAsync();
        Task<ApiResponseDto<bool>> SendEmailNotificationAsync(int queryId);
    }
}
