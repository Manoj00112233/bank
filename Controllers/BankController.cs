using Banking_CapStone.DTO.Request.Bank;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : BaseApiController
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> CreateBank([FromBody] CreateBankRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var superAdminId = GetUserId();
            var result = await _bankService.CreateBankAsync(request, superAdminId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{bankId}")]
        public async Task<IActionResult> GetBank(int bankId)
        {
            var result = await _bankService.GetBankByIdAsync(bankId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{bankId}/details")]
        public async Task<IActionResult> GetBankWithDetails(int bankId)
        {
            var result = await _bankService.GetBankWithDetailsAsync(bankId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetBanksPaginated(
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _bankService.GetBanksPaginatedAsync(pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("superadmin/{superAdminId}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> GetBanksBySuperAdmin(int superAdminId)
        {
            var result = await _bankService.GetBanksBySuperAdminAsync(superAdminId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bankService.UpdateBankAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{bankId}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> DeleteBank(int bankId)
        {
            var result = await _bankService.DeleteBankAsync(bankId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{bankId}/statistics")]
        public async Task<IActionResult> GetBankStatistics(int bankId)
        {
            var result = await _bankService.GetBankStatisticsAsync(bankId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}