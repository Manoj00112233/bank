using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Reports;

namespace Banking_CapStone.Service
{
    public interface IReportService
    {
        Task<ApiResponseDto<byte[]>> GenerateAccountStatementPdfAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateTransactionHistoryExcelAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateClientFinancialSummaryPdfAsync(int clientId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GeneratePaymentReportPdfAsync(int clientId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GeneratePendingPaymentsReportExcelAsync(int? bankId = null);
        Task<ApiResponseDto<byte[]>> GeneratePaymentSummaryByBeneficiaryPdfAsync(int clientId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateSalaryDisbursementReportPdfAsync(int disbursementId);
        Task<ApiResponseDto<byte[]>> GenerateMonthlySalaryReportExcelAsync(int clientId, int month, int year);
        Task<ApiResponseDto<byte[]>> GenerateEmployeeSalarySlipPdfAsync(int employeeId, int month, int year);
        Task<ApiResponseDto<byte[]>> GenerateAnnualSalaryReportPdfAsync(int clientId, int year);
        Task<ApiResponseDto<byte[]>> GenerateBankPerformanceReportPdfAsync(int bankId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateSystemWideDashboardReportPdfAsync(DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateClientOnboardingReportExcelAsync(int bankId, DateTime fromDate, DateTime toDate);

        Task<ApiResponseDto<byte[]>> GenerateAuditTrailReportPdfAsync(int? userId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<byte[]>> GenerateComplianceReportPdfAsync(int bankId, DateTime fromDate, DateTime toDate);

        Task<ApiResponseDto<byte[]>> GenerateCustomReportAsync(string reportType, Dictionary<string, object> parameters);
        Task<ApiResponseDto<string>> GenerateReportUrlAsync(string reportType, Dictionary<string, object> parameters);

        Task<ApiResponseDto<bool>> ValidateReportParametersAsync(string reportType, Dictionary<string, object> parameters);
        Task<ApiResponseDto<List<string>>> GetAvailableReportTypesAsync(string userRole);

        Task<ApiResponseDto<IEnumerable<TransactionReportDto>>> GetTransactionReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<ApiResponseDto<IEnumerable<PaymentReportDto>>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<ApiResponseDto<IEnumerable<AuditLogReportDto>>> GetAuditLogReportAsync(DateTime? fromDate, DateTime? toDate);

    }
}
