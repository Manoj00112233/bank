using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Transaction;

namespace Banking_CapStone.Service
{
    public interface ITransactionService
    {
    Task<ApiResponseDto<TransactionResponseDto>> CreateTransactionAsync(
    int accountId,
    int clientId,
    decimal amount,
    int transactionTypeId,
    int? paymentId = null,
    int? salaryDisbursementId = null,
    int? bankUserId = null,
    string? description = null);
        Task<ApiResponseDto<TransactionResponseDto>> GetTransactionByIdAsync(int transactionId);
        Task<ApiResponseDto<TransactionResponseDto>> GetTransactionWithDetailsAsync(int transactionId);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByAccountIdAsync(int accountId);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByPaymentIdAsync(int paymentId);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsBySalaryDisbursementIdAsync(int disbursementId);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByDateRangeAsync(
            int accountId,
            DateTime fromDate,
            DateTime toDate);
        Task<ApiResponseDto<PaginatedResponseDto<TransactionHistoryResponseDto>>> GetTransactionsPaginatedAsync(
            int? accountId,
            int? clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<AccountStatementResponseDto>> GenerateAccountStatementAsync(
            int accountId,
            DateTime fromDate,
            DateTime toDate);
        Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetRecentTransactionsAsync(int accountId, int count = 10);
        Task<ApiResponseDto<decimal>> GetTotalCreditsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponseDto<decimal>> GetTotalDebitsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponseDto<int>> GetTransactionCountAsync(int clientId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponseDto<Dictionary<string, object>>> GetTransactionStatisticsAsync(int clientId);
        Task<ApiResponseDto<bool>> ProcessCreditTransactionAsync(int accountId, decimal amount, string description, int? paymentId = null);
        Task<ApiResponseDto<bool>> ProcessDebitTransactionAsync(int accountId, decimal amount, string description, int? paymentId = null);
    }
}
