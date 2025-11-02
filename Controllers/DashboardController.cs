using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("superadmin/{superAdminId}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> GetSuperAdminDashboard(int superAdminId)
        {
            var result = await _dashboardService.GetSuperAdminDashboardAsync(superAdminId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("bankuser/{bankUserId}")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> GetBankUserDashboard(int bankUserId)
        {
            var result = await _dashboardService.GetBankUserDashboardAsync(bankUserId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> GetClientDashboard(int clientId)
        {
            var result = await _dashboardService.GetClientDashboardAsync(clientId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("system-statistics")]
        [Authorize(Roles = "SUPER_ADMIN")]
        public async Task<IActionResult> GetSystemWideStatistics()
        {
            var result = await _dashboardService.GetSystemWideStatisticsAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("bank/{bankId}/statistics")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> GetBankStatistics(int bankId)
        {
            var result = await _dashboardService.GetBankStatisticsAsync(bankId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("bank/{bankId}/pending-approvals")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> GetPendingApprovals(int bankId)
        {
            var result = await _dashboardService.GetPendingApprovalsAsync(bankId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/financial-summary")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> GetClientFinancialSummary(int clientId)
        {
            var result = await _dashboardService.GetClientFinancialSummaryAsync(clientId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/recent-transactions")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> GetRecentTransactions(int clientId, [FromQuery] int count = 10)
        {
            var result = await _dashboardService.GetRecentTransactionsAsync(clientId, count);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/upcoming-salaries")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> GetUpcomingSalaryDisbursements(int clientId)
        {
            var result = await _dashboardService.GetUpcomingSalaryDisbursementsAsync(clientId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}