using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Payment;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Payment;

namespace Banking_CapStone.Service
{
    public interface IPaymentService
    {
        Task<ApiResponseDto<PaymentResponseDto>> CreatePaymentAsync(CreatePaymentRequestDto request);
        Task<ApiResponseDto<PaymentDetailsResponseDto>> GetPaymentByIdAsync(int paymentId);
        Task<ApiResponseDto<PaymentDetailsResponseDto>> GetPaymentWithDetailsAsync(int paymentId);
        Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>> GetPendingPaymentsAsync();
        Task<ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>> GetPendingPaymentsByBankIdAsync(int bankId);
        Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetApprovedPaymentsAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetRejectedPaymentsAsync(int clientId);
        Task<ApiResponseDto<PaginatedResponseDto<PaymentResponseDto>>> GetPaymentsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<PaymentResponseDto>> ApprovePaymentAsync(ApprovePaymentRequestDto request);
        Task<ApiResponseDto<PaymentResponseDto>> RejectPaymentAsync(RejectPaymentRequestDto request);
        Task<ApiResponseDto<bool>> ProcessPaymentTransactionAsync(int paymentId);
        Task<ApiResponseDto<decimal>> GetTotalPendingAmountAsync(int clientId);
        Task<ApiResponseDto<int>> GetPendingPaymentsCountAsync(int? clientId = null, int? bankId = null);
        Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByDateRangeAsync(int clientId, DateTime fromDate, DateTime toDate);
        Task<ApiResponseDto<Dictionary<string, object>>> GetPaymentStatisticsAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByBeneficiaryAsync(int beneficiaryId);
    }
}
