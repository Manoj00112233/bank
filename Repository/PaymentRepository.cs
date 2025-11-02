using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Banking_CapStone.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(BankingDbContext context) : base(context) { }

        public async Task<Payment?> GetPaymentWithDetailsAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Client)
                .Include(p => p.Beneficiary)
                .Include(p => p.PaymentStatus)
                .Include(p => p.BankUser)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<Payment?> GetPaymentWithTransactionsAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Transactions)
                .Include(p => p.Client)
                .Include(p => p.Beneficiary)
                .Include(p => p.PaymentStatus)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByClientIdAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .Include(p => p.Beneficiary)
                .Include(p => p.PaymentStatus)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBeneficiaryIdAsync(int beneficiaryId)
        {
            return await _context.Payments
                .Where(p => p.BeneficiaryId == beneficiaryId)
                .Include(p => p.Client)
                .Include(p => p.PaymentStatus)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatusId == 3) // Pending
                .Include(p => p.Client)
                .Include(p => p.Beneficiary)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsByBankIdAsync(int bankId)
        {
            return await _context.Payments
                .Where(p => p.PaymentStatusId == 3 && p.Client.BankId == bankId)
                .Include(p => p.Client)
                .Include(p => p.Beneficiary)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetApprovedPaymentsAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId && p.PaymentStatusId == 1) // Approved
                .Include(p => p.Beneficiary)
                .Include(p => p.BankUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetRejectedPaymentsAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId && p.PaymentStatusId == 2) // Declined
                .Include(p => p.Beneficiary)
                .Include(p => p.BankUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPaymentsPaginatedAsync(
    int? clientId,
    int? bankId,
    PaginationRequestDto pagination,
    FilterRequestDto? filter = null)
        {
            var query = _context.Payments
                .Include(p => p.Client)
                .Include(p => p.Beneficiary)
                .Include(p => p.PaymentStatus)
                .Include(p => p.BankUser)
                .AsQueryable();

            // Apply filters
            if (clientId.HasValue)
                query = query.Where(p => p.ClientId == clientId.Value);

            if (bankId.HasValue)
                query = query.Where(p => p.Client.BankId == bankId.Value);

            if (filter != null)
            {
                if (filter.StatusId.HasValue)
                {
                    query = query.Where(p => p.PaymentStatusId == filter.StatusId.Value);
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(p => p.CreatedAt >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(p => p.CreatedAt <= filter.ToDate.Value);
                }
            }

            
            var totalCount = await query.CountAsync();

           
            var payments = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (payments, totalCount);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, int statusId, int? bankUserId = null)
        {
            var payment = await GetByIdAsync(paymentId);
            if (payment == null) return false;

            payment.PaymentStatusId = statusId;
            if (bankUserId.HasValue)
                payment.BankUserId = bankUserId.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApprovePaymentAsync(int paymentId, int bankUserId)
        {
            var payment = await GetByIdAsync(paymentId);
            if (payment == null || payment.PaymentStatusId != 3) return false; // Not pending

            payment.PaymentStatusId = 1; // Approved
            payment.BankUserId = bankUserId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectPaymentAsync(int paymentId, int bankUserId, string rejectionReason)
        {
            var payment = await GetByIdAsync(paymentId);
            if (payment == null || payment.PaymentStatusId != 3) return false; 

            payment.PaymentStatusId = 2;
            payment.BankUserId = bankUserId;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalPendingAmountAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId && p.PaymentStatusId == 3)
                .SumAsync(p => p.Amount);
        }

        public async Task<int> GetPendingPaymentsCountAsync(int? clientId = null, int? bankId = null)
        {
            var query = _context.Payments.Where(p => p.PaymentStatusId == 3);

            if (clientId.HasValue)
                query = query.Where(p => p.ClientId == clientId.Value);

            if (bankId.HasValue)
                query = query.Where(p => p.Client.BankId == bankId.Value);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(int clientId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId &&
                           p.CreatedAt >= fromDate &&
                           p.CreatedAt <= toDate)
                .Include(p => p.Beneficiary)
                .Include(p => p.PaymentStatus)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaymentStatus?> GetPaymentStatusByIdAsync(int statusId)
        {
            return await _context.PaymentStatuses.FindAsync(statusId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(int statusId, int? clientId = null)
        {
            var query = _context.Payments
                .Where(p => p.PaymentStatusId == statusId)
                .Include(p => p.Client)
                .Include(p => p.Beneficiary);

            if (clientId.HasValue)
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Payment, Beneficiary?>)query.Where(p => p.ClientId == clientId.Value);
            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Dictionary<string, object>> GetPaymentStatisticsAsync(int clientId)
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalPayments"] = await _context.Payments.CountAsync(p => p.ClientId == clientId),
                ["PendingPayments"] = await _context.Payments.CountAsync(p => p.ClientId == clientId && p.PaymentStatusId == 3),
                ["ApprovedPayments"] = await _context.Payments.CountAsync(p => p.ClientId == clientId && p.PaymentStatusId == 1),
                ["RejectedPayments"] = await _context.Payments.CountAsync(p => p.ClientId == clientId && p.PaymentStatusId == 2),
                ["TotalAmountPaid"] = await _context.Payments
                    .Where(p => p.ClientId == clientId && p.PaymentStatusId == 1)
                    .SumAsync(p => p.Amount),
                ["TotalPendingAmount"] = await GetTotalPendingAmountAsync(clientId)
            };

            return stats;
        }
    }
}
