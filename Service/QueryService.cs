using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Query;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Query;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class QueryService : IQueryService
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IEmailService _emailService;
        private readonly IAuditLogService _auditLogService;

        public QueryService(
            IQueryRepository queryRepository,
            IEmailService emailService,
            IAuditLogService auditLogService)
        {
            _queryRepository = queryRepository;
            _emailService = emailService;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<QueryResponseDto>> CreateQueryAsync(CreateQueryRequestDto request)
        {
            try
            {
                var query = new Query
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Subject = request.Subject,
                    Message = request.Message,
                    Category = request.Category,
                    Priority = request.Priority,
                    CreatedAt = DateTime.UtcNow,
                    IsResolved = false
                };

                await _queryRepository.AddAsync(query);

                await _auditLogService.LogCreateAsync("Query", query.QueryId, 0, request.Name);

                return ApiResponseDto<QueryResponseDto>.SuccessResponse(
                    MapToResponseDto(query),
                    "Query created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<QueryResponseDto>.ErrorResponse($"Error creating query: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<QueryResponseDto>> GetQueryByIdAsync(int queryId)
        {
            try
            {
                var query = await _queryRepository.GetQueryWithDetailsAsync(queryId);
                if (query == null)
                    return ApiResponseDto<QueryResponseDto>.ErrorResponse("Query not found");

                return ApiResponseDto<QueryResponseDto>.SuccessResponse(MapToResponseDto(query));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<QueryResponseDto>.ErrorResponse($"Error retrieving query: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<QueryResponseDto>> GetQueryWithDetailsAsync(int queryId)
        {
            return await GetQueryByIdAsync(queryId);
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetAllQueriesAsync()
        {
            try
            {
                var queries = await _queryRepository.GetAllQueriesAsync();
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetPendingQueriesAsync()
        {
            try
            {
                var queries = await _queryRepository.GetPendingQueriesAsync();
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving pending queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetResolvedQueriesAsync()
        {
            try
            {
                var queries = await _queryRepository.GetResolvedQueriesAsync();
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving resolved queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByPriorityAsync(string priority)
        {
            try
            {
                var queries = await _queryRepository.GetQueriesByPriorityAsync(priority);
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByCategoryAsync(string category)
        {
            try
            {
                var queries = await _queryRepository.GetQueriesByCategoryAsync(category);
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<QueryListResponseDto>>> GetQueriesPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (queries, totalCount) = await _queryRepository.GetQueriesPaginatedAsync(pagination, filter);

                var queryDtos = queries.Select(MapToListDto).ToList();

                var paginatedResponse = new PaginatedResponseDto<QueryListResponseDto>(
                    queryDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<QueryListResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<QueryListResponseDto>>.ErrorResponse($"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<QueryResponseDto>> RespondToQueryAsync(RespondToQueryRequestDto request)
        {
            try
            {
                var success = await _queryRepository.RespondToQueryAsync(request.QueryId, request.Response, request.RespondedBy);
                if (!success)
                    return ApiResponseDto<QueryResponseDto>.ErrorResponse("Failed to respond to query");

                if (request.IsResolved)
                    await _queryRepository.MarkAsResolvedAsync(request.QueryId);

                if (request.SendEmailNotification)
                    await SendEmailNotificationAsync(request.QueryId);

                await _auditLogService.LogUpdateAsync("Query", request.QueryId, request.RespondedBy, "User", "Query responded");

                var query = await _queryRepository.GetByIdAsync(request.QueryId);
                return ApiResponseDto<QueryResponseDto>.SuccessResponse(
                    MapToResponseDto(query),
                    "Response sent successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<QueryResponseDto>.ErrorResponse($"Error responding to query: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> MarkAsResolvedAsync(int queryId)
        {
            try
            {
                var success = await _queryRepository.MarkAsResolvedAsync(queryId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Query", queryId, 0, "System", "Query marked as resolved");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Query marked as resolved");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error marking query as resolved: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> MarkAsUnresolvedAsync(int queryId)
        {
            try
            {
                var success = await _queryRepository.MarkAsUnresolvedAsync(queryId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Query", queryId, 0, "System", "Query marked as unresolved");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Query marked as unresolved");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error marking query as unresolved: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetPendingQueriesCountAsync()
        {
            try
            {
                var count = await _queryRepository.GetPendingQueriesCountAsync();
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting pending queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetResolvedQueriesCountAsync()
        {
            try
            {
                var count = await _queryRepository.GetResolvedQueriesCountAsync();
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting resolved queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<QueryListResponseDto>>> GetQueriesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var queries = await _queryRepository.GetQueriesByDateRangeAsync(fromDate, toDate);
                var queryDtos = queries.Select(MapToListDto).ToList();

                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.SuccessResponse(queryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<QueryListResponseDto>>.ErrorResponse($"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetQueryStatisticsAsync()
        {
            try
            {
                var stats = await _queryRepository.GetQueryStatisticsAsync();
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> SendEmailNotificationAsync(int queryId)
        {
            try
            {
                var query = await _queryRepository.GetByIdAsync(queryId);
                if (query == null)
                    return ApiResponseDto<bool>.ErrorResponse("Query not found");

                var success = await _emailService.SendQueryResponseEmailAsync(
                    query.Email,
                    query.Name,
                    query.Subject,
                    query.Response ?? "No response available");

                return ApiResponseDto<bool>.SuccessResponse(success, "Email notification sent");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error sending email: {ex.Message}");
            }
        }

        private QueryResponseDto MapToResponseDto(Query query)
        {
            return new QueryResponseDto
            {
                QueryId = query.QueryId,
                Name = query.Name,
                Email = query.Email,
                Phone = query.Phone,
                Subject = query.Subject,
                Message = query.Message,
                Response = query.Response,
                Category = query.Category ?? "General",
                Priority = query.Priority,
                IsResolved = query.IsResolved,
                CreatedAt = query.CreatedAt,
                RespondedAt = query.RespondedAt,
                RespondedByUserName = query.RespondedByUser?.Username
            };
        }

        private QueryListResponseDto MapToListDto(Query query)
        {
            return new QueryListResponseDto
            {
                QueryId = query.QueryId,
                Subject = query.Subject,
                Category = query.Category ?? "General",
                Priority = query.Priority,
                IsResolved = query.IsResolved,
                CreatedAt = query.CreatedAt,
                DaysPending = (DateTime.UtcNow - query.CreatedAt).Days
            };
        }
    }
}
