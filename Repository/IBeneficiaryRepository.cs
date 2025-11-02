using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IBeneficiaryRepository : IBaseRepository<Beneficiary>
    {
        Task<Beneficiary?> GetBeneficiaryWithDetailsAsync(int beneficiaryId);
        Task<Beneficiary?> GetBeneficiaryWithPaymentsAsync(int beneficiaryId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesByClientIdAsync(int clientId);
        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int clientId);
        Task<IEnumerable<Beneficiary>> GetInactiveBeneficiariesAsync(int clientId);
        Task<(IEnumerable<Beneficiary> Beneficiaries, int TotalCount)> GetBeneficiariesPaginatedAsync(int clientId, PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> IsBeneficiaryExistsAsync(int clientId, string accountNumber, string ifsc);
        Task<bool> ValidateBeneficiaryAsync(int beneficiaryId);
        Task<bool> DeactivateBeneficiaryAsync(int beneficiaryId);
        Task<bool> ActivateBeneficiaryAsync(int beneficiaryId);
        Task<int> GetTotalPaymentsCountAsync(int beneficiaryId);
        Task<decimal> GetTotalAmountPaidAsync(int beneficiaryId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesByBankAsync(int clientId, string bankName);
    }
}
