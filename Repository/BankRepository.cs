using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public BankRepository(BankingDbContext context) : base(context) { }

        public async Task<Bank?> GetBankWithDetailsAsync(int bankId)
        {
            return await _context.Banks
                .Include(b => b.SuperAdmin)
                .Include(b => b.BankUsers)
                .Include(b => b.Clients)
                .FirstOrDefaultAsync(b=> b.BankId == bankId);
        }

        public async Task<Bank?> GetBankWithClientsAsync(int bankId)
        {
            return await _context.Banks
                .Include(b => b.Clients)
                .FirstOrDefaultAsync(b=> b.BankId == bankId);
        }

        public async Task<Bank?> GetBankWithBankUsersAsync(int bankId)
        {
            return await _context.Banks
            .Include(b => b.BankUsers)
            .FirstOrDefaultAsync(b => b.BankId == bankId);
        }

        public async Task<Bank?> GetBankByIFSCAsync(string ifscCode)
        {
            return await _context.Banks
                .FirstOrDefaultAsync(b => b.IFSCCode == ifscCode);
        }

        public async Task<IEnumerable<Bank>> GetActiveBanksAsync()
        {
            return await _context.Banks
                .Where(b => b != null)
                .Include(b => b.SuperAdmin)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bank>> GetBanksBySuperAdminAsync(int superAdminId)
        {
            return await _context.Banks
                .Where(b => b.SuperAdminId == superAdminId)
                .Include(b => b.Clients)
                .Include(b => b.BankUsers)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Bank> Banks, int TotalCount)> GetBanksPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Banks
                .Include(b => b.SuperAdmin)
                .Include(b => b.Clients)
                .Include(b => b.BankUsers)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(b =>
                        b.BankName.Contains(filter.SearchTerm) ||
                        b.IFSCCode.Contains(filter.SearchTerm));
                }
            }

            var totalCount = await query.CountAsync();

            var banks = await query
                .OrderByDescending(b => b.BankId)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (banks, totalCount);
        }

        public async Task<bool> IsIFSCExistsAsync(string ifscCode)
        {
            return await _context.Banks.AnyAsync(b=> b.IFSCCode == ifscCode);
        }

        public async Task<int> GetTotalClientsCountAsync(int bankId)
        {
            return await _context.Clients.CountAsync(c=>c.BankId == bankId);
        }

        public async Task<int> GetTotalBankUsersCountAsync(int bankId)
        {
            return await _context.BankUsers.CountAsync(bu => bu.BankId == bankId);
        }

        public async Task<decimal> GetTotalBankBalanceAsync(int bankId)
        {
            return await _context.Clients
                .Where(c => c.BankId == bankId)
                .SumAsync(c => c.AccountBalance);
        }

        public async Task<Dictionary<string, object>> GetBankStatisticsAsync(int bankId)
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalClients"] = await GetTotalClientsCountAsync(bankId),
                ["TotalBankUsers"] = await GetTotalBankUsersCountAsync(bankId),
                ["TotalBalance"] = await GetTotalBankUsersCountAsync(bankId),
                ["TotalAccounts"] = await _context.Accounts.CountAsync(a => a.BankId == bankId),
                ["TotalTransactions"] = await _context.Transactions
                    .CountAsync(t => t.Account != null && t.Account.BankId == bankId)
            };
            return stats;
        }
    }
}
