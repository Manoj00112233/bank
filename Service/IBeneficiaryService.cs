using Banking_CapStone.DTO.Request.Beneficiary;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Beneficiary;
using Banking_CapStone.DTO.Response.Common;

namespace Banking_CapStone.Service
{
    public interface IBeneficiaryService
    {
        Task<ApiResponseDto<BeneficiaryResponseDto>> CreateBeneficiaryAsync(CreateBeneficiaryRequestDto request);
        Task<ApiResponseDto<BeneficiaryResponseDto>> GetBeneficiaryByIdAsync(int beneficiaryId);
        Task<ApiResponseDto<BeneficiaryResponseDto>> GetBeneficiaryWithDetailsAsync(int beneficiaryId);
        Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetBeneficiariesByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetActiveBeneficiariesAsync(int clientId);
        Task<ApiResponseDto<PaginatedResponseDto<BeneficiaryListResponseDto>>> GetBeneficiariesPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<BeneficiaryResponseDto>> UpdateBeneficiaryAsync(UpdateBeneficiaryRequestDto request);
        Task<ApiResponseDto<bool>> ActivateBeneficiaryAsync(int beneficiaryId);
        Task<ApiResponseDto<bool>> DeactivateBeneficiaryAsync(int beneficiaryId, string reason);
        Task<ApiResponseDto<bool>> ValidateBeneficiaryAsync(int beneficiaryId);
        Task<ApiResponseDto<decimal>> GetTotalAmountPaidToBeneficiaryAsync(int beneficiaryId);
        Task<ApiResponseDto<int>> GetTotalPaymentsCountAsync(int beneficiaryId);
        Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetBeneficiariesByBankAsync(int clientId, string bankName);
    }
}
