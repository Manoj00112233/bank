using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class SalaryDisbursementRepository : BaseRepository<SalaryDisbursement> , ISalaryDisbursementRepository
    {
        public SalaryDisbursementRepository(BankingDbContext context) : base(context) { }

        public async Task<SalaryDisbursement?> GetDisbursementWithDetailsAsync(int disbursementId)
        {
            return await _context.SalaryDisbursements
                .Include(sd => sd.Client)
                .Include(sd => sd.DisbursementStatus)
                .Include(sd => sd.ApprovedBy)
                .Include(sd => sd.DisbursementDetials)
                    .ThenInclude(e=> e.Employee)
                .FirstOrDefaultAsync(sd => sd.SalaryDisbursementId == disbursementId);
        }

        public async Task<SalaryDisbursement?> GetDisbursementWithEmployeesAsync(int disbursementId)
        {
            return await _context.SalaryDisbursements
                .Include(sd => sd.DisbursementDetials)
                    .ThenInclude(d => d.Employee)
                .FirstOrDefaultAsync(sd => sd.SalaryDisbursementId == disbursementId);
        }

        public async Task<IEnumerable<SalaryDisbursement>> GetDisbursementsByClientIdAsync(int clientId)
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.ClientId == clientId)
                .Include(sd => sd.DisbursementStatus)
                .Include(sd => sd.ApprovedBy)
                .OrderByDescending(sd => sd.DisbursementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryDisbursement>> GetPendingDisbursementsAsync()
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.DisbursementStatusId == 3) // Pending
                .Include(sd => sd.Client)
                .OrderBy(sd => sd.DisbursementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryDisbursement>> GetPendingDisbursementsByBankIdAsync(int bankId)
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.DisbursementStatusId == 3 && sd.Client.BankId == bankId)
                .Include(sd => sd.Client)
                .OrderBy(sd => sd.DisbursementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryDisbursement>> GetApprovedDisbursementsAsync(int clientId)
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 1) // Approved
                .Include(sd => sd.ApprovedBy)
                .OrderByDescending(sd => sd.ApprovedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryDisbursement>> GetRejectedDisbursementsAsync(int clientId)
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 2) // Declined
                .Include(sd => sd.ApprovedBy)
                .OrderByDescending(sd => sd.DisbursementDate)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SalaryDisbursement> Disbursements, int TotalCount)> GetDisbursementsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.SalaryDisbursements
                .Include(sd => sd.Client)
                .Include(sd => sd.DisbursementStatus)
                .Include(sd => sd.ApprovedBy)
                .AsQueryable();

            if (clientId.HasValue)
                query = query.Where(sd => sd.ClientId == clientId.Value);

            if (bankId.HasValue)
                query = query.Where(sd => sd.Client.BankId == bankId.Value);

            if (filter != null)
            {
                if (filter.StatusId.HasValue)
                {
                    query = query.Where(sd => sd.DisbursementStatusId == filter.StatusId.Value);
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(sd => sd.DisbursementDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(sd => sd.DisbursementDate <= filter.ToDate.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var disbursements = await query
                .OrderByDescending(sd => sd.DisbursementDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (disbursements, totalCount);
        }

        public async Task<SalaryDisbursement?> GetDisbursementByMonthYearAsync(int clientId, int month, int year)
        {
            return await _context.SalaryDisbursements
                .Include(sd => sd.DisbursementDetials)
                .FirstOrDefaultAsync(sd => sd.ClientId == clientId &&
                    sd.DisbursementDetials.Any(d => d.SalaryMonth == month && d.SalaryYear == year));
        }

        public async Task<bool> IsDisbursementExistsAsync(int clientId, int month, int year)
        {
            return await _context.SalaryDisbursements
                .AnyAsync(sd => sd.ClientId == clientId &&
                    sd.DisbursementDetials.Any(d => d.SalaryMonth == month && d.SalaryYear == year));
        }

        public async Task<bool> ApproveDisbursementAsync(int disbursementId, int bankUserId)
        {
            var disbursement = await GetByIdAsync(disbursementId);
            if (disbursement == null || disbursement.DisbursementStatusId != 3) return false;

            disbursement.DisbursementStatusId = 1; 
            disbursement.ApprovedByBankUserId = bankUserId;
            disbursement.ApprovedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectDisbursementAsync(int disbursementId, int bankUserId, string rejectionReason)
        {
            var disbursement = await GetByIdAsync(disbursementId);
            if (disbursement == null || disbursement.DisbursementStatusId != 3) return false;

            disbursement.DisbursementStatusId = 2; // Declined
            disbursement.ApprovedByBankUserId = bankUserId;
            disbursement.Remarks = rejectionReason;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDisbursementStatusAsync(int disbursementId, int statusId)
        {
            var disbursement = await GetByIdAsync(disbursementId);
            if (disbursement == null) return false;

            disbursement.DisbursementStatusId = statusId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetPendingDisbursementsCountAsync(int? clientId = null, int? bankId = null)
        {
            var query = _context.SalaryDisbursements.Where(sd => sd.DisbursementStatusId == 3);

            if (clientId.HasValue)
                query = query.Where(sd => sd.ClientId == clientId.Value);

            if (bankId.HasValue)
                query = query.Where(sd => sd.Client.BankId == bankId.Value);

            return await query.CountAsync();
        }

        public async Task<decimal> GetTotalPendingDisbursementAmountAsync(int clientId)
        {
            return await _context.SalaryDisbursements
                .Where(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 3)
                .SumAsync(sd => sd.TotalAmount);
        }

        public async Task<Dictionary<string, object>> GetDisbursementStatisticsAsync(int clientId)
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalDisbursements"] = await _context.SalaryDisbursements.CountAsync(sd => sd.ClientId == clientId),
                ["PendingDisbursements"] = await _context.SalaryDisbursements.CountAsync(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 3),
                ["ApprovedDisbursements"] = await _context.SalaryDisbursements.CountAsync(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 1),
                ["RejectedDisbursements"] = await _context.SalaryDisbursements.CountAsync(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 2),
                ["TotalAmountDisbursed"] = await _context.SalaryDisbursements
                    .Where(sd => sd.ClientId == clientId && sd.DisbursementStatusId == 1)
                    .SumAsync(sd => sd.TotalAmount),
                ["TotalPendingAmount"] = await GetTotalPendingDisbursementAmountAsync(clientId)
            };

            return stats;
        }

        public async Task<IEnumerable<SalaryDisbursementDetails>> GetDisbursementDetailsAsync(int disbursementId)
        {
            return await _context.SalaryDisbursementDetails
                .Where(d => d.SalaryDisbursementId == disbursementId)
                .Include(d => d.Employee)
                .Include(d => d.Transaction)
                .ToListAsync();
        }

        public async Task<bool> UpdateDisbursementDetailStatusAsync(int detailId, bool success, string? failureReason = null)
        {
            var detail = await _context.SalaryDisbursementDetails.FindAsync(detailId);
            if (detail == null) return false;

            detail.Success = success;
            detail.FailureReason = failureReason;
            detail.ProcessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
