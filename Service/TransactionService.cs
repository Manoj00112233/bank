using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Transaction;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuditLogService _auditLogService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IAuditLogService auditLogService)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<TransactionResponseDto>> CreateTransactionAsync(
            int accountId,
            int clientId,
            decimal amount,
            int transactionTypeId,
            int? paymentId = null,
            int? salaryDisbursementId = null,
            int? bankUserId = null,
            string? description = null)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                    return ApiResponseDto<TransactionResponseDto>.ErrorResponse("Account not found");

                if (transactionTypeId == 2 && account.Balance < amount)
                    return ApiResponseDto<TransactionResponseDto>.ErrorResponse("Insufficient balance");

                var transaction = new Transaction
                {
                    ClientId = clientId,
                    BankUserId = bankUserId,
                    Amount = amount,
                    AccountId = accountId,
                    TransactionTypeId = transactionTypeId,
                    PaymentId = paymentId,
                    SalaryDisbursementId = salaryDisbursementId,
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow
                };

                await _transactionRepository.AddAsync(transaction);

                if (transactionTypeId == 1)
                    await _accountRepository.CreditAccountAsync(accountId, amount);
                else if (transactionTypeId == 2)
                    await _accountRepository.DebitAccountAsync(accountId, amount);

                await _auditLogService.LogCreateAsync("Transaction", transaction.TransactionId, clientId, "System");

                return ApiResponseDto<TransactionResponseDto>.SuccessResponse(
                    await MapToTransactionResponseDto(transaction),
                    "Transaction created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TransactionResponseDto>.ErrorResponse($"Error creating transaction: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TransactionResponseDto>> GetTransactionByIdAsync(int transactionId)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionWithDetailsAsync(transactionId);
                if (transaction == null)
                    return ApiResponseDto<TransactionResponseDto>.ErrorResponse("Transaction not found");

                return ApiResponseDto<TransactionResponseDto>.SuccessResponse(await MapToTransactionResponseDto(transaction));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<TransactionResponseDto>.ErrorResponse($"Error retrieving transaction: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<TransactionResponseDto>> GetTransactionWithDetailsAsync(int transactionId)
        {
            return await GetTransactionByIdAsync(transactionId);
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByAccountIdAsync(int accountId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByClientIdAsync(int clientId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByClientIdAsync(clientId);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByPaymentIdAsync(int paymentId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByPaymentIdAsync(paymentId);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsBySalaryDisbursementIdAsync(int disbursementId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsBySalaryDisbursementIdAsync(disbursementId);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetTransactionsByDateRangeAsync(
            int accountId,
            DateTime fromDate,
            DateTime toDate)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(accountId, fromDate, toDate);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<TransactionHistoryResponseDto>>> GetTransactionsPaginatedAsync(
            int? accountId,
            int? clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (transactions, totalCount) = await _transactionRepository.GetTransactionsPaginatedAsync(accountId, clientId, pagination, filter);

                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                var paginatedResponse = new PaginatedResponseDto<TransactionHistoryResponseDto>(
                    transactionDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<TransactionHistoryResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AccountStatementResponseDto>> GenerateAccountStatementAsync(
            int accountId,
            DateTime fromDate,
            DateTime toDate)
        {
            try
            {
                var statement = await _transactionRepository.GenerateAccountStatementAsync(accountId, fromDate, toDate);

                var statementDto = new AccountStatementResponseDto
                {
                    AccountNumber = statement["AccountNumber"].ToString(),
                    ClientName = "Client",
                    FromDate = fromDate,
                    ToDate = toDate,
                    OpeningBalance = (decimal)statement["OpeningBalance"],
                    ClosingBalance = (decimal)statement["ClosingBalance"],
                    TotalCredits = (decimal)statement["TotalCredits"],
                    TotalDebits = (decimal)statement["TotalDebits"],
                    TotalTransactions = (int)statement["TotalTransactions"],
                    Transactions = ((IEnumerable<Transaction>)statement["Transactions"]).Select(MapToTransactionHistoryDto).ToList()
                };

                return ApiResponseDto<AccountStatementResponseDto>.SuccessResponse(statementDto);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AccountStatementResponseDto>.ErrorResponse($"Error generating statement: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>> GetRecentTransactionsAsync(int accountId, int count = 10)
        {
            try
            {
                var transactions = await _transactionRepository.GetRecentTransactionsAsync(accountId, count);
                var transactionDtos = transactions.Select(MapToTransactionHistoryDto).ToList();

                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.SuccessResponse(transactionDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<TransactionHistoryResponseDto>>.ErrorResponse($"Error retrieving transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalCreditsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var total = await _transactionRepository.GetTotalCreditsByAccountAsync(accountId, fromDate, toDate);
                return ApiResponseDto<decimal>.SuccessResponse(total);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating credits: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalDebitsByAccountAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var total = await _transactionRepository.GetTotalDebitsByAccountAsync(accountId, fromDate, toDate);
                return ApiResponseDto<decimal>.SuccessResponse(total);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating debits: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetTransactionCountAsync(int clientId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var count = await _transactionRepository.GetTransactionCountByClientAsync(clientId, fromDate, toDate);
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting transactions: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetTransactionStatisticsAsync(int clientId)
        {
            try
            {
                var stats = await _transactionRepository.GetTransactionStatisticsAsync(clientId);
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ProcessCreditTransactionAsync(int accountId, decimal amount, string description, int? paymentId = null)
        {
            var result = await CreateTransactionAsync(accountId, 0, amount, 1, paymentId, null, null, description);
            return ApiResponseDto<bool>.SuccessResponse(result.Success);
        }

        public async Task<ApiResponseDto<bool>> ProcessDebitTransactionAsync(int accountId, decimal amount, string description, int? paymentId = null)
        {
            var result = await CreateTransactionAsync(accountId, 0, amount, 2, paymentId, null, null, description);
            return ApiResponseDto<bool>.SuccessResponse(result.Success);
        }

        private async Task<TransactionResponseDto> MapToTransactionResponseDto(Transaction transaction)
        {
            return new TransactionResponseDto
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId ?? 0,
                AccountNumber = transaction.Account?.AccountNumber ?? "Unknown",
                ClientId = transaction.ClientId,
                ClientName = transaction.Client?.ClientName ?? "Unknown",
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType?.Type.ToString() ?? "Unknown",
                Status = transaction.Status,
                PaymentId = transaction.PaymentId,
                SalaryDisbursementId = transaction.SalaryDisbursementId,
                ReferenceNumber = null,
                CreatedAt = transaction.CreatedAt
            };
        }

        private TransactionHistoryResponseDto MapToTransactionHistoryDto(Transaction transaction)
        {
            return new TransactionHistoryResponseDto
            {
                TransactionId = transaction.TransactionId,
                AccountNumber = transaction.Account?.AccountNumber ?? "Unknown",
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType?.Type.ToString() ?? "Unknown",
                Status = transaction.Status,
                Description = "Transaction",
                BalanceAfter = 0,
                TransactionDate = transaction.CreatedAt,
                ReferenceNumber = null
            };
        }
    }
}
