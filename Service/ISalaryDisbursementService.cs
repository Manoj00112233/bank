using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Salary_Disbursement;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.SalaryDisbursement;

namespace Banking_CapStone.Service
{
    public interface ISalaryDisbursementService
    {
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> CreateSalaryDisbursementAsync(CreateSalaryDisbursementRequestDto request);
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementByIdAsync(int disbursementId);
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementWithDetailsAsync(int disbursementId);
        Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetDisbursementsByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetPendingDisbursementsAsync();
        Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetPendingDisbursementsByBankIdAsync(int bankId);
        Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetApprovedDisbursementsAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetRejectedDisbursementsAsync(int clientId);
        Task<ApiResponseDto<PaginatedResponseDto<SalaryDisbursementListResponseDto>>> GetDisbursementsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> ApproveDisbursementAsync(ApproveSalaryDisbursementRequestDto request);
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> RejectDisbursementAsync(RejectSalaryDisbursementRequestDto request);
        Task<ApiResponseDto<bool>> ProcessDisbursementAsync(int disbursementId);
        Task<ApiResponseDto<bool>> ProcessBatchDisbursementAsync(int disbursementId);
        Task<ApiResponseDto<int>> GetPendingDisbursementsCountAsync(int? clientId = null, int? bankId = null);
        Task<ApiResponseDto<decimal>> GetTotalPendingDisbursementAmountAsync(int clientId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetDisbursementStatisticsAsync(int clientId);
        Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementByMonthYearAsync(int clientId, int month, int year);
        Task<ApiResponseDto<bool>> ValidateDisbursementRequestAsync(CreateSalaryDisbursementRequestDto request);
    }
}
