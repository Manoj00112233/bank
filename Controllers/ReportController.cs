using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : BaseApiController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("account-statement/{accountId}")]
        public async Task<IActionResult> DownloadAccountStatement(
            int accountId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _reportService.GenerateAccountStatementPdfAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"AccountStatement_{accountId}_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("transaction-history-excel/{accountId}")]
        public async Task<IActionResult> DownloadTransactionHistoryExcel(
            int accountId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _reportService.GenerateTransactionHistoryExcelAsync(accountId, fromDate, toDate);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"TransactionHistory_{accountId}_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        [HttpGet("client-financial-summary/{clientId}")]
        public async Task<IActionResult> DownloadClientFinancialSummary(
            int clientId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _reportService.GenerateClientFinancialSummaryPdfAsync(clientId, fromDate, toDate);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"FinancialSummary_{clientId}_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("salary-disbursement/{disbursementId}")]
        [Authorize(Roles = "BANK_USER,CLIENT_USER")]
        public async Task<IActionResult> DownloadSalaryDisbursementReport(int disbursementId)
        {
            var result = await _reportService.GenerateSalaryDisbursementReportPdfAsync(disbursementId);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"SalaryDisbursement_{disbursementId}_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("monthly-salary/{clientId}")]
        [Authorize(Roles = "CLIENT_USER")]
        public async Task<IActionResult> DownloadMonthlySalaryReport(
            int clientId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var result = await _reportService.GenerateMonthlySalaryReportExcelAsync(clientId, month, year);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"MonthlySalary_{clientId}_{month}_{year}.xlsx");
        }

        [HttpGet("salary-slip/{employeeId}")]
        public async Task<IActionResult> DownloadEmployeeSalarySlip(
            int employeeId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var result = await _reportService.GenerateEmployeeSalarySlipPdfAsync(employeeId, month, year);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"SalarySlip_{employeeId}_{month}_{year}.pdf");
        }

        [HttpGet("pending-payments-excel")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> DownloadPendingPaymentsReport([FromQuery] int? bankId = null)
        {
            var result = await _reportService.GeneratePendingPaymentsReportExcelAsync(bankId);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"PendingPayments_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        [HttpGet("audit-trail")]
        [Authorize(Roles = "SUPER_ADMIN,BANK_USER")]
        public async Task<IActionResult> DownloadAuditTrailReport(
            [FromQuery] int? userId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var result = await _reportService.GenerateAuditTrailReportPdfAsync(userId, fromDate, toDate);

            if (!result.Success)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"AuditTrail_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("available-reports")]
        public async Task<IActionResult> GetAvailableReports()
        {
            var userRole = GetUserRole();
            var result = await _reportService.GetAvailableReportTypesAsync(userRole);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.GetTransactionReportAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.GetPaymentReportAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.GetAuditLogReportAsync(fromDate, toDate);
            return Ok(result);
        }
    }
}