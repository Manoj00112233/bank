using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransaction(int transactionId)
        {
            var result = await _transactionService.GetTransactionByIdAsync(transactionId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{transactionId}/details")]
        public async Task<IActionResult> GetTransactionWithDetails(int transactionId)
        {
            var result = await _transactionService.GetTransactionWithDetailsAsync(transactionId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccount(int accountId)
        {
            var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetTransactionsByClient(int clientId)
        {
            var result = await _transactionService.GetTransactionsByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("payment/{paymentId}")]
        public async Task<IActionResult> GetTransactionsByPayment(int paymentId)
        {
            var result = await _transactionService.GetTransactionsByPaymentIdAsync(paymentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}/date-range")]
        public async Task<IActionResult> GetTransactionsByDateRange(
            int accountId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _transactionService.GetTransactionsByDateRangeAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetTransactionsPaginated(
            [FromQuery] int? accountId,
            [FromQuery] int? clientId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _transactionService.GetTransactionsPaginatedAsync(accountId, clientId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}/statement")]
        public async Task<IActionResult> GenerateAccountStatement(
            int accountId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _transactionService.GenerateAccountStatementAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}/recent")]
        public async Task<IActionResult> GetRecentTransactions(int accountId, [FromQuery] int count = 10)
        {
            var result = await _transactionService.GetRecentTransactionsAsync(accountId, count);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}/total-credits")]
        public async Task<IActionResult> GetTotalCredits(
            int accountId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _transactionService.GetTotalCreditsByAccountAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("account/{accountId}/total-debits")]
        public async Task<IActionResult> GetTotalDebits(
            int accountId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _transactionService.GetTotalDebitsByAccountAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/statistics")]
        public async Task<IActionResult> GetTransactionStatistics(int clientId)
        {
            var result = await _transactionService.GetTransactionStatisticsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
