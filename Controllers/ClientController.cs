using Banking_CapStone.DTO.Request.Client;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : BaseApiController
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("onboard")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> OnboardClient([FromBody] ClientOnboardingRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.OnboardClientAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.CreateClientAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClient(int clientId)
        {
            var result = await _clientService.GetClientByIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{clientId}/accounts")]
        public async Task<IActionResult> GetClientWithAccounts(int clientId)
        {
            var result = await _clientService.GetClientWithAccountsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{clientId}/employees")]
        public async Task<IActionResult> GetClientWithEmployees(int clientId)
        {
            var result = await _clientService.GetClientWithEmployeesAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{clientId}/beneficiaries")]
        public async Task<IActionResult> GetClientWithBeneficiaries(int clientId)
        {
            var result = await _clientService.GetClientWithBeneficiariesAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("bank/{bankId}/list")]
        public async Task<IActionResult> GetClientsPaginated(
            int bankId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _clientService.GetClientsPaginatedAsync(bankId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "BANK_USER,CLIENT_USER")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.UpdateClientAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{clientId}/verify")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> VerifyClient(int clientId)
        {
            var bankUserId = GetUserId();
            var result = await _clientService.VerifyClientAsync(clientId, bankUserId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{clientId}/deactivate")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> DeactivateClient(int clientId)
        {
            var result = await _clientService.DeactivateClientAsync(clientId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{clientId}/statistics")]
        public async Task<IActionResult> GetClientStatistics(int clientId)
        {
            var result = await _clientService.GetClientStatisticsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
