using Banking_CapStone.DTO.Response.Reports;

namespace Banking_CapStone.Repository
{
    public interface IReportRepository
    {
        Task<IEnumerable<TransactionReportDto>> GetTransactionReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<PaymentReportDto>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<AuditLogReportDto>> GetAuditLogReportAsync(DateTime? fromDate, DateTime? toDate);
    }
}