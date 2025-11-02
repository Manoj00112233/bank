using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Payment;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.CreatePaymentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(int paymentId)
        {
            var result = await _paymentService.GetPaymentByIdAsync(paymentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{paymentId}/details")]
        public async Task<IActionResult> GetPaymentWithDetails(int paymentId)
        {
            var result = await _paymentService.GetPaymentWithDetailsAsync(paymentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> GetPaymentsByClient(int clientId)
        {
            var result = await _paymentService.GetPaymentsByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "BANK_USER,SUPER_ADMIN")]
        public async Task<IActionResult> GetPendingPayments()
        {
            var result = await _paymentService.GetPendingPaymentsAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("pending/bank/{bankId}")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> GetPendingPaymentsByBank(int bankId)
        {
            var result = await _paymentService.GetPendingPaymentsByBankIdAsync(bankId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/approved")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> GetApprovedPayments(int clientId)
        {
            var result = await _paymentService.GetApprovedPaymentsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/rejected")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> GetRejectedPayments(int clientId)
        {
            var result = await _paymentService.GetRejectedPaymentsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetPaymentsPaginated(
            [FromQuery] int? clientId,
            [FromQuery] int? bankId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _paymentService.GetPaymentsPaginatedAsync(clientId, bankId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("approve")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> ApprovePayment([FromBody] ApprovePaymentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.ApprovePaymentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reject")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> RejectPayment([FromBody] RejectPaymentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.RejectPaymentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/pending-amount")]
        public async Task<IActionResult> GetTotalPendingAmount(int clientId)
        {
            var result = await _paymentService.GetTotalPendingAmountAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/statistics")]
        public async Task<IActionResult> GetPaymentStatistics(int clientId)
        {
            var result = await _paymentService.GetPaymentStatisticsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
