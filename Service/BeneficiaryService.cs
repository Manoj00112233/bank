using Banking_CapStone.DTO.Request.Beneficiary;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Beneficiary;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAuditLogService _auditLogService;

        public BeneficiaryService(
            IBeneficiaryRepository beneficiaryRepository,
            IClientRepository clientRepository,
            IAuditLogService auditLogService)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _clientRepository = clientRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<BeneficiaryResponseDto>> CreateBeneficiaryAsync(CreateBeneficiaryRequestDto request)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Client not found");

                if (await _beneficiaryRepository.IsBeneficiaryExistsAsync(request.ClientId, request.AccountNumber, request.IFSC))
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Beneficiary already exists");

                var beneficiary = new Beneficiary
                {
                    ClientId = request.ClientId,
                    BeneficiaryName = request.BeneficiaryName,
                    AccountNumber = request.AccountNumber,
                    BankName = request.BankName,
                    IFSC = request.IFSC,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    IsActive = request.ActivateImmediately,
                    CreatedAt = DateTime.UtcNow
                };

                await _beneficiaryRepository.AddAsync(beneficiary);

                await _auditLogService.LogCreateAsync("Beneficiary", beneficiary.BeneficiaryId, request.ClientId, client.Username);

                return ApiResponseDto<BeneficiaryResponseDto>.SuccessResponse(
                    await MapToResponseDto(beneficiary),
                    "Beneficiary created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse($"Error creating beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BeneficiaryResponseDto>> GetBeneficiaryByIdAsync(int beneficiaryId)
        {
            try
            {
                var beneficiary = await _beneficiaryRepository.GetByIdAsync(beneficiaryId);
                if (beneficiary == null)
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Beneficiary not found");

                return ApiResponseDto<BeneficiaryResponseDto>.SuccessResponse(await MapToResponseDto(beneficiary));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse($"Error retrieving beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BeneficiaryResponseDto>> GetBeneficiaryWithDetailsAsync(int beneficiaryId)
        {
            try
            {
                var beneficiary = await _beneficiaryRepository.GetBeneficiaryWithDetailsAsync(beneficiaryId);
                if (beneficiary == null)
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Beneficiary not found");

                return ApiResponseDto<BeneficiaryResponseDto>.SuccessResponse(await MapToResponseDto(beneficiary));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse($"Error retrieving beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetBeneficiariesByClientIdAsync(int clientId)
        {
            try
            {
                var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByClientIdAsync(clientId);
                var beneficiaryDtos = beneficiaries.Select(b => new BeneficiaryListResponseDto
                {
                    BeneficiaryId = b.BeneficiaryId,
                    BeneficiaryName = b.BeneficiaryName,
                    AccountNumber = b.AccountNumber,
                    BankName = b.BankName,
                    IsActive = b.IsActive
                }).ToList();

                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.SuccessResponse(beneficiaryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.ErrorResponse($"Error retrieving beneficiaries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetActiveBeneficiariesAsync(int clientId)
        {
            try
            {
                var beneficiaries = await _beneficiaryRepository.GetActiveBeneficiariesAsync(clientId);
                var beneficiaryDtos = beneficiaries.Select(b => new BeneficiaryListResponseDto
                {
                    BeneficiaryId = b.BeneficiaryId,
                    BeneficiaryName = b.BeneficiaryName,
                    AccountNumber = b.AccountNumber,
                    BankName = b.BankName,
                    IsActive = b.IsActive
                }).ToList();

                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.SuccessResponse(beneficiaryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.ErrorResponse($"Error retrieving beneficiaries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<BeneficiaryListResponseDto>>> GetBeneficiariesPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (beneficiaries, totalCount) = await _beneficiaryRepository.GetBeneficiariesPaginatedAsync(
                    clientId, pagination, filter);

                var beneficiaryDtos = beneficiaries.Select(b => new BeneficiaryListResponseDto
                {
                    BeneficiaryId = b.BeneficiaryId,
                    BeneficiaryName = b.BeneficiaryName,
                    AccountNumber = b.AccountNumber,
                    BankName = b.BankName,
                    IsActive = b.IsActive
                }).ToList();

                var paginatedResponse = new PaginatedResponseDto<BeneficiaryListResponseDto>(
                    beneficiaryDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<BeneficiaryListResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<BeneficiaryListResponseDto>>.ErrorResponse($"Error retrieving beneficiaries: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BeneficiaryResponseDto>> UpdateBeneficiaryAsync(UpdateBeneficiaryRequestDto request)
        {
            try
            {
                var beneficiary = await _beneficiaryRepository.GetByIdAsync(request.BeneficiaryId);
                if (beneficiary == null)
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Beneficiary not found");

                if (beneficiary.ClientId != request.ClientId)
                    return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse("Unauthorized");

                if (!string.IsNullOrEmpty(request.BeneficiaryName))
                    beneficiary.BeneficiaryName = request.BeneficiaryName;

                if (!string.IsNullOrEmpty(request.Email))
                    beneficiary.Email = request.Email;

                if (!string.IsNullOrEmpty(request.Phone))
                    beneficiary.Phone = request.Phone;

                if (!string.IsNullOrEmpty(request.Address))
                    beneficiary.Address = request.Address;

                if (request.IsActive.HasValue)
                    beneficiary.IsActive = request.IsActive.Value;

                beneficiary.UpdatedAt = DateTime.UtcNow;

                await _beneficiaryRepository.UpdateAsync(beneficiary);

                await _auditLogService.LogUpdateAsync("Beneficiary", beneficiary.BeneficiaryId, request.ClientId, "Client");

                return ApiResponseDto<BeneficiaryResponseDto>.SuccessResponse(
                    await MapToResponseDto(beneficiary),
                    "Beneficiary updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BeneficiaryResponseDto>.ErrorResponse($"Error updating beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ActivateBeneficiaryAsync(int beneficiaryId)
        {
            try
            {
                var success = await _beneficiaryRepository.ActivateBeneficiaryAsync(beneficiaryId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Beneficiary", beneficiaryId, 0, "System", "Beneficiary activated");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Beneficiary activated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error activating beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeactivateBeneficiaryAsync(int beneficiaryId, string reason)
        {
            try
            {
                var success = await _beneficiaryRepository.DeactivateBeneficiaryAsync(beneficiaryId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Beneficiary", beneficiaryId, 0, "System", $"Beneficiary deactivated. Reason: {reason}");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Beneficiary deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error deactivating beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ValidateBeneficiaryAsync(int beneficiaryId)
        {
            try
            {
                var isValid = await _beneficiaryRepository.ValidateBeneficiaryAsync(beneficiaryId);
                return ApiResponseDto<bool>.SuccessResponse(isValid);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error validating beneficiary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalAmountPaidToBeneficiaryAsync(int beneficiaryId)
        {
            try
            {
                var totalAmount = await _beneficiaryRepository.GetTotalAmountPaidAsync(beneficiaryId);
                return ApiResponseDto<decimal>.SuccessResponse(totalAmount);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating total amount: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetTotalPaymentsCountAsync(int beneficiaryId)
        {
            try
            {
                var count = await _beneficiaryRepository.GetTotalPaymentsCountAsync(beneficiaryId);
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting payments: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>> GetBeneficiariesByBankAsync(int clientId, string bankName)
        {
            try
            {
                var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByBankAsync(clientId, bankName);
                var beneficiaryDtos = beneficiaries.Select(b => new BeneficiaryListResponseDto
                {
                    BeneficiaryId = b.BeneficiaryId,
                    BeneficiaryName = b.BeneficiaryName,
                    AccountNumber = b.AccountNumber,
                    BankName = b.BankName,
                    IsActive = b.IsActive
                }).ToList();

                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.SuccessResponse(beneficiaryDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<BeneficiaryListResponseDto>>.ErrorResponse($"Error retrieving beneficiaries: {ex.Message}");
            }
        }

        private async Task<BeneficiaryResponseDto> MapToResponseDto(Beneficiary beneficiary)
        {
            var client = beneficiary.Client ?? await _clientRepository.GetByIdAsync(beneficiary.ClientId);
            var totalPayments = await _beneficiaryRepository.GetTotalPaymentsCountAsync(beneficiary.BeneficiaryId);
            var totalAmount = await _beneficiaryRepository.GetTotalAmountPaidAsync(beneficiary.BeneficiaryId);

            return new BeneficiaryResponseDto
            {
                BeneficiaryId = beneficiary.BeneficiaryId,
                BeneficiaryName = beneficiary.BeneficiaryName,
                AccountNumber = beneficiary.AccountNumber,
                BankName = beneficiary.BankName,
                IFSC = beneficiary.IFSC,
                Email = beneficiary.Email,
                Phone = beneficiary.Phone,
                Address = beneficiary.Address,
                IsActive = beneficiary.IsActive,
                ClientId = beneficiary.ClientId,
                ClientName = client?.ClientName ?? "Unknown",
                TotalPaymentsMade = totalPayments,
                TotalAmountPaid = totalAmount,
                CreatedAt = beneficiary.CreatedAt,
                UpdatedAt = beneficiary.UpdatedAt,
            };
        }
    }
}