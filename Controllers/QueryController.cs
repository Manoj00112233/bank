using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Query;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : BaseApiController
    {
        private readonly IQueryService _queryService;

        public QueryController(IQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateQuery([FromBody] CreateQueryRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _queryService.CreateQueryAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{queryId}")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetQuery(int queryId)
        {
            var result = await _queryService.GetQueryByIdAsync(queryId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetAllQueries()
        {
            var result = await _queryService.GetAllQueriesAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetPendingQueries()
        {
            var result = await _queryService.GetPendingQueriesAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("resolved")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetResolvedQueries()
        {
            var result = await _queryService.GetResolvedQueriesAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("priority/{priority}")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetQueriesByPriority(string priority)
        {
            var result = await _queryService.GetQueriesByPriorityAsync(priority);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetQueriesPaginated(
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _queryService.GetQueriesPaginatedAsync(pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("respond")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> RespondToQuery([FromBody] RespondToQueryRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _queryService.RespondToQueryAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{queryId}/resolve")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> MarkAsResolved(int queryId)
        {
            var result = await _queryService.MarkAsResolvedAsync(queryId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetQueryStatistics()
        {
            var result = await _queryService.GetQueryStatisticsAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}