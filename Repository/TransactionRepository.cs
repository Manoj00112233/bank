using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankingDbContext context) : base(context) { }

        public async Task<Transaction?> GetTransactionWithDetailsAsync(int transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Client)
                .Include(t => t.Account)
                .Include(t => t.TransactionType)
                .Include(t => t.Payment)
                .Include(t => t.SalaryDisbursement)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByClientIdAsync(int clientId)
        {
            return await _context.Transactions
                .Where(t => t.ClientId == clientId)
                .Include(t => t.Account)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBankUserIdAsync(int bankUserId)
        {
            return await _context.Transactions
                .Where(t => t.BankUserId == bankUserId)
                .Include(t => t.Client)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByPaymentIdAsync(int paymentId)
        {
            return await _context.Transactions
                .Where(t => t.PaymentId == paymentId)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsBySalaryDisbursementIdAsync(int disbursementId)
        {
            return await _context.Transactions
                .Where(t => t.SalaryDisbursementId == disbursementId)
                .Include(t => t.TransactionType)
                .Include(t => t.SalaryDisbursementDetail)
                    .ThenInclude(d => d.Employee)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId &&
                           t.CreatedAt >= fromDate &&
                           t.CreatedAt <= toDate)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionTypeId, int? accountId = null)
        {
            var query = _context.Transactions
                .Where(t => t.TransactionTypeId == transactionTypeId)
                .Include(t => t.Account)
                .Include(t => t.Client);

            if (accountId.HasValue)
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Transaction, Client?>)query.Where(t => t.AccountId == accountId.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetTransactionsPaginatedAsync(
            int? accountId,
            int? clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Client)
                .Include(t => t.TransactionType)
                .AsQueryable();

            // Apply filters
            if (accountId.HasValue)
                query = query.Where(t => t.AccountId == accountId.Value);

            if (clientId.HasValue)
                query = query.Where(t => t.ClientId == clientId.Value);

            if (filter != null)
            {
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(t => t.CreatedAt >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(t => t.CreatedAt <= filter.ToDate.Value);
                }

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(t => t.Status.Contains(filter.SearchTerm));
                }
            }

            var totalCount = await query.CountAsync();

          
            var transactions = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (transactions, totalCount);
        }

        public async Task<decimal> GetTotalCreditsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Transactions
                .Where(t => t.AccountId == accountId && t.TransactionTypeId == 1); // Credit

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            return await query.SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalDebitsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Transactions
                .Where(t => t.AccountId == accountId && t.TransactionTypeId == 2); // Debit

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            return await query.SumAsync(t => t.Amount);
        }


        public async Task<int> GetTransactionCountByClientAsync(int clientId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Transactions.Where(t => t.ClientId == clientId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            return await query.CountAsync();
        }

        public async Task<Dictionary<string, object>> GenerateAccountStatementAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            var transactions = await GetTransactionsByDateRangeAsync(accountId, fromDate, toDate);

            var credits = await GetTotalCreditsByAccountAsync(accountId, fromDate, toDate);
            var debits = await GetTotalDebitsByAccountAsync(accountId, fromDate, toDate);

            var statement = new Dictionary<string, object>
            {
                ["AccountNumber"] = account?.AccountNumber ?? "N/A",
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate,
                ["OpeningBalance"] = account?.Balance ?? 0 - credits + debits,
                ["ClosingBalance"] = account?.Balance ?? 0,
                ["TotalCredits"] = credits,
                ["TotalDebits"] = debits,
                ["TotalTransactions"] = transactions.Count(),
                ["Transactions"] = transactions
            };

            return statement;
        }

        public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(int accountId, int count = 10)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<TransactionType?> GetTransactionTypeByIdAsync(int transactionTypeId)
        {
            return await _context.TransactionTypes.FindAsync(transactionTypeId);
        }

        public async Task<Dictionary<string, object>> GetTransactionStatisticsAsync(int clientId)
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalTransactions"] = await _context.Transactions.CountAsync(t => t.ClientId == clientId),
                ["TotalCredits"] = await _context.Transactions
                    .Where(t => t.ClientId == clientId && t.TransactionTypeId == 1)
                    .SumAsync(t => t.Amount),
                ["TotalDebits"] = await _context.Transactions
                    .Where(t => t.ClientId == clientId && t.TransactionTypeId == 2)
                    .SumAsync(t => t.Amount),
                ["TransactionsToday"] = await _context.Transactions
                    .CountAsync(t => t.ClientId == clientId && t.CreatedAt.Date == DateTime.UtcNow.Date),
                ["TransactionsThisMonth"] = await _context.Transactions
                    .CountAsync(t => t.ClientId == clientId &&
                        t.CreatedAt.Year == DateTime.UtcNow.Year &&
                        t.CreatedAt.Month == DateTime.UtcNow.Month)
            };

            return stats;
        }
    }
}
