using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<Transaction?> GetTransactionWithDetailsAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByClientIdAsync(int clientId);
        Task<IEnumerable<Transaction>> GetTransactionsByBankUserIdAsync(int bankUserId);
        Task<IEnumerable<Transaction>> GetTransactionsByPaymentIdAsync(int paymentId);
        Task<IEnumerable<Transaction>> GetTransactionsBySalaryDisbursementIdAsync(int disbursementId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionTypeId, int? accountId = null);
        Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetTransactionsPaginatedAsync(
            int? accountId,
            int? clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<decimal> GetTotalCreditsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<decimal> GetTotalDebitsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<int> GetTransactionCountByClientAsync(int clientId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<Dictionary<string, object>> GenerateAccountStatementAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(int accountId, int count = 10);
        Task<TransactionType?> GetTransactionTypeByIdAsync(int transactionTypeId);
        Task<Dictionary<string, object>> GetTransactionStatisticsAsync(int clientId);       
        
    }
}
