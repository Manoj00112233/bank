using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<Account?> GetAccountWithDetailsAsync(int accountId);
        Task<Account?> GetAccountWithTransactionsAsync(int accountId);
        Task<Account?> GetAccountByNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAccountsByClientIdAsync(int clientId);
        Task<IEnumerable<Account>> GetAccountsByBankIdAsync(int bankId);
        Task<IEnumerable<Account>> GetActiveAccountsAsync(int clientId);
        Task<(IEnumerable<Account> Accounts, int TotalCount)> GetAccountsPaginatedAsync(int? clientId, int? bankId, PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> IsAccountNumberExistsAsync(string accountNumber);
        Task<bool> UpdateAccountBalanceAsync(int accountId, decimal newBalance);
        Task<bool> CreditAccountAsync(int accountId, decimal amount);
        Task<bool> DebitAccountAsync(int accountId, decimal amount);
        Task<bool> UpdateAccountStatusAsync(int accountId, int statusId, string reason, int updatedByBankUserId);
        Task<decimal> GetAccountBalanceAsync(int accountId);
        Task<decimal> GetTotalBalanceByClientIdAsync(int clientId);
        Task<IEnumerable<Account>> GetAccountsByTypeAsync(int accountTypeId);
        Task<IEnumerable<Account>> GetAccountsByStatusAsync(int accountStatusId);
        Task<AccountType?> GetAccountTypeByIdAsync(int accountTypeId);
        Task<AccountStatus?> GetAccountStatusByIdAsync(int accountStatusId);
    }
}
