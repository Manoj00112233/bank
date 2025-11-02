using Banking_CapStone.Data;
using Banking_CapStone.Model;
using Banking_CapStone.DTO.Request.Common;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(BankingDbContext context) : base(context)
        {
        }

        public async Task<Client?> GetClientWithDetailsAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Bank)
                .Include(c => c.Accounts)
                .Include(c => c.Employees)
                .Include(c => c.Beneficiaries)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Client?> GetClientWithAccountsAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.AccountType)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.AccountStatus)
                .FirstOrDefaultAsync(c => c.ClientId  == clientId);
        }

        public async Task<Client?> GetClientWithEmployeesAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Employees.Where(e => e.IsActive))
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Client?> GetClientWithBeneficiariesAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Beneficiaries.Where(b => b.IsActive))
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Client?> GetClientByUsernameAsync(string username)
        {
            return await _context.Clients
                .Include(c => c.Bank)
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<IEnumerable<Client>> GetClientsByBankIdAsync(int bankId)
        {
            return await _context.Clients
                .Where(c => c.BankId == bankId)
                .Include(c => c.Accounts)
                .ToListAsync();
        }

        public async Task<Client?> GetClientByPanAsync(string panNumber)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.PanNumber == panNumber);
        }

        public async Task<IEnumerable<Client>> GetActiveClientsAsync(int bankId)
        {
            return await _context.Clients
                .Where(c => c.BankId == bankId && c.IsActive)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Client> Clients, int TotalCount)> GetClientsPaginatedAsync(
            int bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Clients
                .Where(c => c.BankId == bankId)
                .Include(c => c.Bank)
                .AsQueryable();

           
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(c =>
                        c.ClientName.Contains(filter.SearchTerm) ||
                        c.Email.Contains(filter.SearchTerm)); 
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(c => c.IsActive == filter.IsActive.Value);
                }
            }

            var totalCount = await query.CountAsync();

        
            var clients = await query
                .OrderByDescending(c => c.ClientId)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (clients, totalCount);
        }
        public async Task<bool> IsPanExistsAsync(string panNumber)
        {
            return await _context.Clients.AnyAsync(c => c.PanNumber == panNumber);
        }

        public async Task<bool> IsGstExistsAsync(string gstNumber)
        {
            return await _context.Clients.AnyAsync(c => c.GstNumber == gstNumber);
        }
        public async Task<bool> VerifyClientAsync(int clientId, int verifiedByBankUserId)
        {
            var client = await GetByIdAsync(clientId);
            if (client == null) return false;

            client.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetClientBalanceAsync(int clientId)
        {
            var client = await GetByIdAsync(clientId);
            return client?.AccountBalance ?? 0;
        }

        public async Task<bool> UpdateClientBalanceAsync(int clientId, decimal newBalance)
        {
            var client = await GetByIdAsync(clientId);
            if (client == null) return false;

            client.AccountBalance = newBalance;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalEmployeesCountAsync(int clientId)
        {
            return await _context.Employees.CountAsync(e => e.ClientId == clientId);
        }

        public async Task<int> GetTotalBeneficiariesCountAsync(int clientId)
        {
            return await _context.Beneficiaries.CountAsync(b => b.ClientId == clientId);
        }

        public async Task<Dictionary<string, object>> GetClientStatisticsAsync(int clientId)
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalBalance"] = await GetClientBalanceAsync(clientId),
                ["TotalEmployees"] = await GetTotalEmployeesCountAsync(clientId),
                ["TotalBeneficiaries"] = await GetTotalBeneficiariesCountAsync(clientId),
                ["TotalAccounts"] = await _context.Accounts.CountAsync(a => a.ClientId == clientId),
                ["TotalPayments"] = await _context.Payments.CountAsync(p => p.ClientId == clientId),
                ["PendingPayments"] = await _context.Payments
                    .CountAsync(p => p.ClientId == clientId && p.PaymentStatusId == 3), // Pending
                ["TotalTransactions"] = await _context.Transactions.CountAsync(t => t.ClientId == clientId)
            };

            return stats;
        }

    }
}
