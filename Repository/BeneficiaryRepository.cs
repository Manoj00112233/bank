using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class BeneficiaryRepository : BaseRepository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(BankingDbContext context) : base(context) { }

        public async Task<Beneficiary?> GetBeneficiaryWithDetailsAsync(int beneficiaryId)
        {
            return await _context.Beneficiaries
                .Include(b => b.Client)
                .FirstOrDefaultAsync(b => b.BeneficiaryId == beneficiaryId);
        }

        public async Task<Beneficiary?> GetBeneficiaryWithPaymentsAsync(int beneficiaryId)
        {
            return await _context.Beneficiaries
                    .Include (b => b.Payments)
                    .FirstOrDefaultAsync(b => b.BeneficiaryId == beneficiaryId);
        }
        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesByClientIdAsync(int clientId)
        {
            return await _context.Beneficiaries
                .Where(b => b.ClientId == clientId)
                .OrderBy(b => b.BeneficiaryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int clientId)
        {
            return await _context.Beneficiaries
                .Where(b => b.ClientId == clientId && b.IsActive)
                .OrderBy(b => b.BeneficiaryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Beneficiary>> GetInactiveBeneficiariesAsync(int clientId)
        {
            return await _context.Beneficiaries
                .Where(b => b.ClientId == clientId && !b.IsActive)
                .OrderBy(b => b.BeneficiaryName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Beneficiary> Beneficiaries, int TotalCount)> GetBeneficiariesPaginatedAsync(
           int clientId,
           PaginationRequestDto pagination,
           FilterRequestDto? filter = null)
        {
            var query = _context.Beneficiaries
                .Where(b => b.ClientId == clientId)
                .Include(b => b.Client)
                .AsQueryable();

            
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(b =>
                        b.BeneficiaryName.Contains(filter.SearchTerm) ||
                        b.AccountNumber.Contains(filter.SearchTerm) ||
                        b.BankName.Contains(filter.SearchTerm));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(b => b.IsActive == filter.IsActive.Value);
                }
            }

            var totalCount = await query.CountAsync();

            
            var beneficiaries = await query
                .OrderBy(b => b.BeneficiaryName)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (beneficiaries, totalCount);
        }

        public async Task<bool> IsBeneficiaryExistsAsync(int clientId, string accountNumber, string ifsc)
        {
            return await _context.Beneficiaries
                .AnyAsync(b => b.ClientId == clientId &&
                              b.AccountNumber == accountNumber &&
                              b.IFSC == ifsc);
        }

        public async Task<bool> ValidateBeneficiaryAsync(int beneficiaryId)
        {
            var beneficiary = await GetByIdAsync(beneficiaryId);
            return beneficiary != null && beneficiary.IsActive;
        }

        public async Task<bool> DeactivateBeneficiaryAsync(int beneficiaryId)
        {
            var beneficiary = await GetByIdAsync(beneficiaryId);
            if (beneficiary == null) return false;

            beneficiary.IsActive = false;
            beneficiary.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateBeneficiaryAsync(int beneficiaryId)
        {
            var beneficiary = await GetByIdAsync(beneficiaryId);
            if (beneficiary == null) return false;

            beneficiary.IsActive = true;
            beneficiary.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalPaymentsCountAsync(int beneficiaryId)
        {
            return await _context.Payments
                .CountAsync(p => p.BeneficiaryId == beneficiaryId);
        }

        public async Task<decimal> GetTotalAmountPaidAsync(int beneficiaryId)
        {
            return await _context.Payments
                 .Where(p => p.BeneficiaryId == beneficiaryId && p.PaymentStatusId == 1) // Approved
                 .SumAsync(p => p.Amount);
        }

        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesByBankAsync(int clientId, string bankName)
        {
            return await _context.Beneficiaries
                .Where(b => b.ClientId == clientId && b.BankName == bankName && b.IsActive)
                .ToListAsync();
        }
    }
}
