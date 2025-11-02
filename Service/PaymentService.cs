using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Payment;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Payment;
using Banking_CapStone.DTO.Response.Transaction;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ITransactionService _transactionService;
        private readonly IAuditLogService _auditLogService;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IClientRepository clientRepository,
            IBeneficiaryRepository beneficiaryRepository,
            ITransactionService transactionService,
            IAuditLogService auditLogService)
        {
            _paymentRepository = paymentRepository;
            _clientRepository = clientRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _transactionService = transactionService;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<PaymentResponseDto>> CreatePaymentAsync(CreatePaymentRequestDto request)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Client not found");

                var beneficiary = await _beneficiaryRepository.GetByIdAsync(request.BeneficiaryId);
                if (beneficiary == null)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Beneficiary not found");

                if (!beneficiary.IsActive)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Beneficiary is not active");

                var payment = new Payment
                {
                    ClientId = request.ClientId,
                    BeneficiaryId = request.BeneficiaryId,
                    Amount = request.Amount,
                    PaymentStatusId = 3,
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRepository.AddAsync(payment);

                await _auditLogService.LogCreateAsync("Payment", payment.PaymentId, request.ClientId, "Client");

                return ApiResponseDto<PaymentResponseDto>.SuccessResponse(
                    await MapToPaymentResponseDto(payment),
                    "Payment created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaymentResponseDto>.ErrorResponse($"Error creating payment: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaymentDetailsResponseDto>> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentWithDetailsAsync(paymentId);
                if (payment == null)
                    return ApiResponseDto<PaymentDetailsResponseDto>.ErrorResponse("Payment not found");

                return ApiResponseDto<PaymentDetailsResponseDto>.SuccessResponse(await MapToPaymentDetailsDto(payment));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaymentDetailsResponseDto>.ErrorResponse($"Error retrieving payment: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaymentDetailsResponseDto>> GetPaymentWithDetailsAsync(int paymentId)
        {
            return await GetPaymentByIdAsync(paymentId);
        }

        public async Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByClientIdAsync(int clientId)
        {
            try
            {
                var payments = await _paymentRepository.GetPaymentsByClientIdAsync(clientId);
                var paymentDtos = new List<PaymentResponseDto>();

                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.SuccessResponse(paymentDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.ErrorResponse($"Error retrieving payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>> GetPendingPaymentsAsync()
        {
            try
            {
                var payments = await _paymentRepository.GetPendingPaymentsAsync();
                var pendingDtos = payments.Select(p => new PendingPaymentsResponseDto
                {
                    PaymentId = p.PaymentId,
                    ClientName = p.Client?.ClientName ?? "Unknown",
                    BeneficiaryName = p.Beneficiary?.BeneficiaryName ?? "Unknown",
                    Amount = p.Amount,
                    PaymentPurpose = "Payment",
                    CreatedAt = p.CreatedAt,
                    DaysPending = (DateTime.UtcNow - p.CreatedAt).Days,
                    IsUrgent = (DateTime.UtcNow - p.CreatedAt).Days > 3
                }).ToList();

                return ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>.SuccessResponse(pendingDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>.ErrorResponse($"Error retrieving pending payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>> GetPendingPaymentsByBankIdAsync(int bankId)
        {
            try
            {
                var payments = await _paymentRepository.GetPendingPaymentsByBankIdAsync(bankId);
                var pendingDtos = payments.Select(p => new PendingPaymentsResponseDto
                {
                    PaymentId = p.PaymentId,
                    ClientName = p.Client?.ClientName ?? "Unknown",
                    BeneficiaryName = p.Beneficiary?.BeneficiaryName ?? "Unknown",
                    Amount = p.Amount,
                    PaymentPurpose = "Payment",
                    CreatedAt = p.CreatedAt,
                    DaysPending = (DateTime.UtcNow - p.CreatedAt).Days,
                    IsUrgent = (DateTime.UtcNow - p.CreatedAt).Days > 3
                }).ToList();

                return ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>.SuccessResponse(pendingDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PendingPaymentsResponseDto>>.ErrorResponse($"Error retrieving pending payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetApprovedPaymentsAsync(int clientId)
        {
            try
            {
                var payments = await _paymentRepository.GetApprovedPaymentsAsync(clientId);
                var paymentDtos = new List<PaymentResponseDto>();

                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.SuccessResponse(paymentDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.ErrorResponse($"Error retrieving approved payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetRejectedPaymentsAsync(int clientId)
        {
            try
            {
                var payments = await _paymentRepository.GetRejectedPaymentsAsync(clientId);
                var paymentDtos = new List<PaymentResponseDto>();

                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.SuccessResponse(paymentDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.ErrorResponse($"Error retrieving rejected payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<PaymentResponseDto>>> GetPaymentsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (payments, totalCount) = await _paymentRepository.GetPaymentsPaginatedAsync(clientId, bankId, pagination, filter);

                var paymentDtos = new List<PaymentResponseDto>();
                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                var paginatedResponse = new PaginatedResponseDto<PaymentResponseDto>(
                    paymentDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<PaymentResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<PaymentResponseDto>>.ErrorResponse($"Error retrieving payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaymentResponseDto>> ApprovePaymentAsync(ApprovePaymentRequestDto request)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(request.paymentId);
                if (payment == null)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Payment not found");

                if (payment.PaymentStatusId != 3)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Payment is not pending");

                var success = await _paymentRepository.ApprovePaymentAsync(request.paymentId, request.BankUserId);
                if (!success)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Failed to approve payment");

                await ProcessPaymentTransactionAsync(request.paymentId);

                await _auditLogService.LogPaymentApprovalAsync(request.paymentId, request.BankUserId, "BankUser", true);

                payment = await _paymentRepository.GetByIdAsync(request.paymentId);
                return ApiResponseDto<PaymentResponseDto>.SuccessResponse(
                    await MapToPaymentResponseDto(payment),
                    "Payment approved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaymentResponseDto>.ErrorResponse($"Error approving payment: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaymentResponseDto>> RejectPaymentAsync(RejectPaymentRequestDto request)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
                if (payment == null)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Payment not found");

                if (payment.PaymentStatusId != 3)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Payment is not pending");

                var success = await _paymentRepository.RejectPaymentAsync(request.PaymentId, request.BankUserId, request.RejectionReason);
                if (!success)
                    return ApiResponseDto<PaymentResponseDto>.ErrorResponse("Failed to reject payment");

                await _auditLogService.LogPaymentApprovalAsync(request.PaymentId, request.BankUserId, "BankUser", false);

                payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
                return ApiResponseDto<PaymentResponseDto>.SuccessResponse(
                    await MapToPaymentResponseDto(payment),
                    "Payment rejected successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaymentResponseDto>.ErrorResponse($"Error rejecting payment: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ProcessPaymentTransactionAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentWithDetailsAsync(paymentId);
                if (payment == null)
                    return ApiResponseDto<bool>.ErrorResponse("Payment not found");

                var clientAccounts = await _clientRepository.GetClientWithAccountsAsync(payment.ClientId);
                if (clientAccounts?.Accounts == null || !clientAccounts.Accounts.Any())
                    return ApiResponseDto<bool>.ErrorResponse("No active account found");

                var account = clientAccounts.Accounts.FirstOrDefault(a => a.AccountStatusId == 1);
                if (account == null)
                    return ApiResponseDto<bool>.ErrorResponse("No active account found");

                await _transactionService.CreateTransactionAsync(
                    account.AccountId,
                    payment.ClientId,
                    payment.Amount,
                    2,
                    paymentId,
                    null,
                    payment.BankUserId,
                    $"Payment to {payment.Beneficiary?.BeneficiaryName}");

                return ApiResponseDto<bool>.SuccessResponse(true, "Payment transaction processed");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error processing transaction: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalPendingAmountAsync(int clientId)
        {
            try
            {
                var amount = await _paymentRepository.GetTotalPendingAmountAsync(clientId);
                return ApiResponseDto<decimal>.SuccessResponse(amount);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating pending amount: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetPendingPaymentsCountAsync(int? clientId = null, int? bankId = null)
        {
            try
            {
                var count = await _paymentRepository.GetPendingPaymentsCountAsync(clientId, bankId);
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting pending payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByDateRangeAsync(int clientId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(clientId, fromDate, toDate);
                var paymentDtos = new List<PaymentResponseDto>();

                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.SuccessResponse(paymentDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.ErrorResponse($"Error retrieving payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetPaymentStatisticsAsync(int clientId)
        {
            try
            {
                var stats = await _paymentRepository.GetPaymentStatisticsAsync(clientId);
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<PaymentResponseDto>>> GetPaymentsByBeneficiaryAsync(int beneficiaryId)
        {
            try
            {
                var payments = await _paymentRepository.GetPaymentsByBeneficiaryIdAsync(beneficiaryId);
                var paymentDtos = new List<PaymentResponseDto>();

                foreach (var payment in payments)
                {
                    paymentDtos.Add(await MapToPaymentResponseDto(payment));
                }

                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.SuccessResponse(paymentDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<PaymentResponseDto>>.ErrorResponse($"Error retrieving payments: {ex.Message}");
            }
        }

        private async Task<PaymentResponseDto> MapToPaymentResponseDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                ClientId = payment.ClientId,
                ClientName = payment.Client?.ClientName ?? "Unknown",
                BeneficiaryId = payment.BeneficiaryId,
                BeneficiaryName = payment.Beneficiary?.BeneficiaryName,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus?.Status.ToString() ?? "Unknown",
                PaymentPurpose = "Payment",
                ReferenceNumber = null,
                CreatedAt = payment.CreatedAt,
                ApprovedAt = null,
                ApprovedByBankUserId = payment.BankUserId,
                ApprovedByBankUserName = payment.BankUser?.FullName,
                Remarks = null
            };
        }

        private async Task<PaymentDetailsResponseDto> MapToPaymentDetailsDto(Payment payment)
        {
            return new PaymentDetailsResponseDto
            {
                PaymentId = payment.PaymentId,
                ClientId = payment.ClientId,
                ClientName = payment.Client?.ClientName ?? "Unknown",
                ClientAccountNumber = payment.Client?.AccountNumber ?? "Unknown",
                BeneficiaryId = payment.BeneficiaryId,
                BeneficiaryName = payment.Beneficiary?.BeneficiaryName ?? "Unknown",
                BeneficiaryAccountNumber = payment.Beneficiary?.AccountNumber ?? "Unknown",
                BeneficiaryBankName = payment.Beneficiary?.BankName ?? "Unknown",
                BeneficiaryIFSC = payment.Beneficiary?.IFSC ?? "Unknown",
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus?.Status.ToString() ?? "Unknown",
                PaymentPurpose = "Payment",
                ReferenceNumber = null,
                CreatedAt = payment.CreatedAt,
                ApprovedAt = null,
                ApprovedByBankUserName = payment.BankUser?.FullName,
                ApprovalRemarks = null,
                RejectionReason = null,
                Transactions = new List<TransactionSummaryDto>()
            };
        }
    }
}