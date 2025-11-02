using Banking_CapStone.DTO.Request.Beneficiary;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CLIENT_USER")]
    public class BeneficiaryController : BaseApiController
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBeneficiary([FromBody] CreateBeneficiaryRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _beneficiaryService.CreateBeneficiaryAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{beneficiaryId}")]
        public async Task<IActionResult> GetBeneficiary(int beneficiaryId)
        {
            var result = await _beneficiaryService.GetBeneficiaryByIdAsync(beneficiaryId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetBeneficiariesByClient(int clientId)
        {
            var result = await _beneficiaryService.GetBeneficiariesByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/active")]
        public async Task<IActionResult> GetActiveBeneficiaries(int clientId)
        {
            var result = await _beneficiaryService.GetActiveBeneficiariesAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("client/{clientId}/list")]
        public async Task<IActionResult> GetBeneficiariesPaginated(
            int clientId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _beneficiaryService.GetBeneficiariesPaginatedAsync(clientId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBeneficiary([FromBody] UpdateBeneficiaryRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _beneficiaryService.UpdateBeneficiaryAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{beneficiaryId}/activate")]
        public async Task<IActionResult> ActivateBeneficiary(int beneficiaryId)
        {
            var result = await _beneficiaryService.ActivateBeneficiaryAsync(beneficiaryId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{beneficiaryId}/deactivate")]
        public async Task<IActionResult> DeactivateBeneficiary(int beneficiaryId, [FromQuery] string reason)
        {
            var result = await _beneficiaryService.DeactivateBeneficiaryAsync(beneficiaryId, reason);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{beneficiaryId}/total-paid")]
        public async Task<IActionResult> GetTotalAmountPaid(int beneficiaryId)
        {
            var result = await _beneficiaryService.GetTotalAmountPaidToBeneficiaryAsync(beneficiaryId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}