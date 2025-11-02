using Banking_CapStone.DTO.Request.Account;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.CreateAccountAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var result = await _accountService.GetAccountByIdAsync(accountId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("number/{accountNumber}")]
        public async Task<IActionResult> GetAccountByNumber(string accountNumber)
        {
            var result = await _accountService.GetAccountByNumberAsync(accountNumber);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetAccountsByClient(int clientId)
        {
            var result = await _accountService.GetAccountsByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/active")]
        public async Task<IActionResult> GetActiveAccounts(int clientId)
        {
            var result = await _accountService.GetActiveAccountsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetAccountBalance(int accountId)
        {
            var result = await _accountService.GetAccountBalanceAsync(accountId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAccountsPaginated(
            [FromQuery] int? clientId,
            [FromQuery] int? bankId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _accountService.GetAccountsPaginatedAsync(clientId, bankId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("status")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> UpdateAccountStatus([FromBody] UpdateAccountStatusRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.UpdateAccountStatusAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/total-balance")]
        public async Task<IActionResult> GetTotalBalance(int clientId)
        {
            var result = await _accountService.GetTotalBalanceByClientAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}