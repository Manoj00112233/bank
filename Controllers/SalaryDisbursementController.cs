using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Salary_Disbursement;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalaryDisbursementController : BaseApiController
    {
        private readonly ISalaryDisbursementService _salaryDisbursementService;

        public SalaryDisbursementController(ISalaryDisbursementService salaryDisbursementService)
        {
            _salaryDisbursementService = salaryDisbursementService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> CreateSalaryDisbursement([FromBody] CreateSalaryDisbursementRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _salaryDisbursementService.CreateSalaryDisbursementAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{disbursementId}")]
        public async Task<IActionResult> GetDisbursement(int disbursementId)
        {
            var result = await _salaryDisbursementService.GetDisbursementByIdAsync(disbursementId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{disbursementId}/details")]
        public async Task<IActionResult> GetDisbursementWithDetails(int disbursementId)
        {
            var result = await _salaryDisbursementService.GetDisbursementWithDetailsAsync(disbursementId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> GetDisbursementsByClient(int clientId)
        {
            var result = await _salaryDisbursementService.GetDisbursementsByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "BANK_USER,SUPER_ADMIN")]
        public async Task<IActionResult> GetPendingDisbursements()
        {
            var result = await _salaryDisbursementService.GetPendingDisbursementsAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pending/bank/{bankId}")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> GetPendingDisbursementsByBank(int bankId)
        {
            var result = await _salaryDisbursementService.GetPendingDisbursementsByBankIdAsync(bankId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/approved")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> GetApprovedDisbursements(int clientId)
        {
            var result = await _salaryDisbursementService.GetApprovedDisbursementsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetDisbursementsPaginated(
            [FromQuery] int? clientId,
            [FromQuery] int? bankId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _salaryDisbursementService.GetDisbursementsPaginatedAsync(clientId, bankId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("approve")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> ApproveDisbursement([FromBody] ApproveSalaryDisbursementRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _salaryDisbursementService.ApproveDisbursementAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reject")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> RejectDisbursement([FromBody] RejectSalaryDisbursementRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _salaryDisbursementService.RejectDisbursementAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/statistics")]
        public async Task<IActionResult> GetDisbursementStatistics(int clientId)
        {
            var result = await _salaryDisbursementService.GetDisbursementStatisticsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}