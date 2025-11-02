using Banking_CapStone.Data;
using Banking_CapStone.DTO.Response.Reports;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly BankingDbContext _context;

        public ReportRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionReportDto>> GetTransactionReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Transactions
                .Include(t => t.Client)
                .Include(t => t.Account)
                    .ThenInclude(a => a.Bank)
                .Include(t => t.TransactionType)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            var result = await query
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionReportDto
                {
                    TransactionId = t.TransactionId,
                    ClientName = t.Client != null ? t.Client.ClientName : "N/A",
                    BankName = t.Account != null && t.Account.Bank != null ? t.Account.Bank.BankName : "N/A",
                    Amount = t.Amount,
                    TransactionType = t.TransactionType != null ? t.TransactionType.Type.ToString() : "N/A",
                    Status = t.Status,
                    TransactionDate = t.CreatedAt
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<PaymentReportDto>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Payments
                .Include(p => p.Client)
                    .ThenInclude(c => c.Bank)
                .Include(p => p.PaymentStatus)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(p => p.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(p => p.CreatedAt <= toDate.Value);

            var result = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PaymentReportDto
                {
                    PaymentId = p.PaymentId,
                    ClientName = p.Client != null ? p.Client.ClientName : "N/A",
                    BankName = p.Client != null && p.Client.Bank != null ? p.Client.Bank.BankName : "N/A",
                    Amount = p.Amount,
                    Status = p.PaymentStatus != null ? p.PaymentStatus.Status.ToString() : "N/A",
                    PaymentDate = p.CreatedAt
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AuditLogReportDto>> GetAuditLogReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.AuditLogs
                .Include(a => a.User)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(a => a.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(a => a.Timestamp <= toDate.Value);

            var result = await query
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogReportDto
                {
                    LogId = a.AuditLogId,
                    UserId = a.UserId,
                    Username = a.User != null ? a.User.Username : "N/A",
                    Action = a.ActionType,
                    Entity = a.Details ?? "N/A",
                    Timestamp = a.Timestamp
                })
                .ToListAsync();

            return result;
        }
    }
}