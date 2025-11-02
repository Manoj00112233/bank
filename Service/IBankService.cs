using Banking_CapStone.DTO.Request.Bank;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Bank;
using Banking_CapStone.DTO.Response.Common;

namespace Banking_CapStone.Service
{
    public interface IBankService
    {
        Task<ApiResponseDto<BankResponseDto>> CreateBankAsync(CreateBankRequestDto request, int superAdminId);
        Task<ApiResponseDto<BankResponseDto>> GetBankByIdAsync(int bankId);
        Task<ApiResponseDto<BankResponseDto>> GetBankWithDetailsAsync(int bankId);
        Task<ApiResponseDto<PaginatedResponseDto<BankListResponseDto>>> GetBanksPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);

        Task<ApiResponseDto<IEnumerable<BankListResponseDto>>> GetBanksBySuperAdminAsync(int superAdminId);
        Task<ApiResponseDto<BankResponseDto>> UpdateBankAsync(UpdateBankRequestDto request);
        Task<ApiResponseDto<bool>> DeleteBankAsync(int bankId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetBankStatisticsAsync(int bankId);
        Task<ApiResponseDto<bool>> IsBankActiveAsync(int bankId);
    }
}
