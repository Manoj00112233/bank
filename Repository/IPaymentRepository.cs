using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetPaymentWithDetailsAsync(int paymentId);
        Task<Payment?> GetPaymentWithTransactionsAsync(int paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByClientIdAsync(int clientId);
        Task<IEnumerable<Payment>> GetPaymentsByBeneficiaryIdAsync(int beneficiaryId);
        Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
        Task<IEnumerable<Payment>> GetPendingPaymentsByBankIdAsync(int bankId);
        Task<IEnumerable<Payment>> GetApprovedPaymentsAsync(int clientId);
        Task<IEnumerable<Payment>> GetRejectedPaymentsAsync(int clientId);
        Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPaymentsPaginatedAsync(int? clientId, int? bankId, PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, int statusId, int? bankUserId = null);
        Task<bool> ApprovePaymentAsync(int paymentId, int bankUserId);
        Task<bool> RejectPaymentAsync(int paymentId, int bankUserId, string rejectionReason);
        Task<decimal> GetTotalPendingAmountAsync(int clientId);
        Task<int> GetPendingPaymentsCountAsync(int? clientId = null, int? bankId = null);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(int clientId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(int statusId, int? clientId = null);
        Task<PaymentStatus?> GetPaymentStatusByIdAsync(int statusId);
        Task<Dictionary<string, object>> GetPaymentStatisticsAsync(int clientId);
    }
}
