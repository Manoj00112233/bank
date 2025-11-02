using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface ISalaryDisbursementRepository : IBaseRepository<SalaryDisbursement>
    {
        Task<SalaryDisbursement?> GetDisbursementWithDetailsAsync(int disbursementId);
        Task<SalaryDisbursement?> GetDisbursementWithEmployeesAsync(int disbursementId);
        Task<IEnumerable<SalaryDisbursement>> GetDisbursementsByClientIdAsync(int clientId);
        Task<IEnumerable<SalaryDisbursement>> GetPendingDisbursementsAsync();
        Task<IEnumerable<SalaryDisbursement>> GetPendingDisbursementsByBankIdAsync(int bankId);
        Task<IEnumerable<SalaryDisbursement>> GetApprovedDisbursementsAsync(int clientId);
        Task<IEnumerable<SalaryDisbursement>> GetRejectedDisbursementsAsync(int clientId);
        Task<(IEnumerable<SalaryDisbursement> Disbursements, int TotalCount)> GetDisbursementsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<SalaryDisbursement?> GetDisbursementByMonthYearAsync(int clientId, int month, int year);
        Task<bool> IsDisbursementExistsAsync(int clientId, int month, int year);
        Task<bool> ApproveDisbursementAsync(int disbursementId, int bankUserId);
        Task<bool> RejectDisbursementAsync(int disbursementId, int bankUserId, string rejectionReason);
        Task<bool> UpdateDisbursementStatusAsync(int disbursementId, int statusId);
        Task<int> GetPendingDisbursementsCountAsync(int? clientId = null, int? bankId = null);
        Task<decimal> GetTotalPendingDisbursementAmountAsync(int clientId);
        Task<Dictionary<string, object>> GetDisbursementStatisticsAsync(int clientId);
        Task<IEnumerable<SalaryDisbursementDetails>> GetDisbursementDetailsAsync(int disbursementId);
        Task<bool> UpdateDisbursementDetailStatusAsync(int detailId, bool success, string? failureReason = null);
    }
}
