using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankingDbContext context) : base(context)
        {
        }

        public async Task<Account?> GetAccountWithDetailsAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Client)
                .Include(a => a.Bank)
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<Account?> GetAccountWithTransactionsAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Transactions.OrderByDescending(t => t.CreatedAt))
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .Include(a => a.Client)
                .Include(a => a.Bank)
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<IEnumerable<Account>> GetAccountsByClientIdAsync(int clientId)
        {
            return await _context.Accounts
                .Where(a => a.ClientId == clientId)
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountsByBankIdAsync(int bankId)
        {
            return await _context.Accounts
                .Where(a => a.BankId == bankId)
                .Include(a => a.Client)
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetActiveAccountsAsync(int clientId)
        {
            return await _context.Accounts
                .Where(a => a.ClientId == clientId && a.AccountStatusId == 1) // Active
                .Include(a => a.AccountType)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Account> Accounts, int TotalCount)> GetAccountsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Accounts
                .Include(a => a.Client)
                .Include(a => a.Bank)
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .AsQueryable();

            if(clientId.HasValue)
                query = query.Where(a => a.ClientId == clientId);

            if(bankId.HasValue)
                query = query.Where(a=>  a.BankId == bankId);

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(a => a.AccountNumber.Contains(filter.SearchTerm));
                }

                if (filter.StatusId.HasValue)
                {
                    query = query.Where(a => a.AccountStatusId == filter.StatusId.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var accounts = await query 
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (accounts, totalCount);
        }

        public async Task<bool> IsAccountNumberExistsAsync(string accountNumber)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<bool> UpdateAccountBalanceAsync(int accountId, decimal newBalance)
        {
            var account = await GetByIdAsync(accountId);
            if (account == null) return false;

            account.Balance = newBalance;
            account.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreditAccountAsync(int accountId, decimal amount)
        {
            var account = await GetByIdAsync(accountId);
            if (account == null || amount <= 0) return false;

            account.Balance += amount;
            account.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DebitAccountAsync(int accountId, decimal amount)
        {
            var account = await GetByIdAsync(accountId);
            if (account == null || amount <= 0 || account.Balance < amount)
                return false;

            account.Balance -= amount;
            account.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccountStatusAsync(int accountId, int statusId, string reason, int updatedByBankUserId)
        {
            var account = await GetByIdAsync(accountId);
            if (account == null) return false;

            account.AccountStatusId = statusId;
            account.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var account = await GetByIdAsync(accountId);
            return account?.Balance ?? 0;
        }

        public async Task<decimal> GetTotalBalanceByClientIdAsync(int clientId)
        {
            return await _context.Accounts
                .Where(a => a.ClientId == clientId && a.AccountStatusId == 1)                 .SumAsync(a => a.Balance);
        }

        public async Task<IEnumerable<Account>> GetAccountsByTypeAsync(int accountTypeId)
        {
            return await _context.Accounts
                .Where(a => a.AccountTypeId == accountTypeId)
                .Include(a => a.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountsByStatusAsync(int accountStatusId)
        {
            return await _context.Accounts
                .Where(a => a.AccountStatusId == accountStatusId)
                .Include(a => a.Client)
                .ToListAsync();
        }

        public async Task<AccountType?> GetAccountTypeByIdAsync(int accountTypeId)
        {
            return await _context.AccountTypes.FindAsync(accountTypeId);
        }

        public async Task<AccountStatus?> GetAccountStatusByIdAsync(int accountStatusId)
        {
            return await _context.AccountStatuses.FindAsync(accountStatusId);
        }
    }
}
