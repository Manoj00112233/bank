
using Banking_CapStone.DTO.Request.Bank;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Bank;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IAuditLogService _auditLogService;

        public BankService(
            IBankRepository bankRepository,
            IAuthRepository authRepository,
            IAuditLogService auditLogService)
        {
            _bankRepository = bankRepository;
            _authRepository = authRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<BankResponseDto>> CreateBankAsync(CreateBankRequestDto request, int superAdminId)
        {
            try
            {
                if (await _bankRepository.IsIFSCExistsAsync(request.IFSCCode))
                    return ApiResponseDto<BankResponseDto>.ErrorResponse("IFSC code already exists");

                var superAdmin = await _authRepository.GetUserByIdAsync(superAdminId);
                if (superAdmin == null || superAdmin.RoleId != 1)
                    return ApiResponseDto<BankResponseDto>.ErrorResponse("Invalid super admin");

                var bank = new Bank
                {
                    BankName = request.BankName,
                    IFSCCode = request.IFSCCode,
                    //Address = request.Address ?? "",
                    ContactNumber = request.ContactNumber,
                    SupportEmail = request.SupportEmail,
                    SuperAdminId = superAdminId
                };

                await _bankRepository.AddAsync(bank);

                await _auditLogService.LogCreateAsync("Bank", bank.BankId, superAdminId, superAdmin.Username);

                return ApiResponseDto<BankResponseDto>.SuccessResponse(
                    await MapToResponseDto(bank),
                    "Bank created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BankResponseDto>.ErrorResponse($"Error creating bank: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BankResponseDto>> GetBankByIdAsync(int bankId)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(bankId);
                if (bank == null)
                    return ApiResponseDto<BankResponseDto>.ErrorResponse("Bank not found");

                return ApiResponseDto<BankResponseDto>.SuccessResponse(await MapToResponseDto(bank));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BankResponseDto>.ErrorResponse($"Error retrieving bank: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BankResponseDto>> GetBankWithDetailsAsync(int bankId)
        {
            try
            {
                var bank = await _bankRepository.GetBankWithDetailsAsync(bankId);
                if (bank == null)
                    return ApiResponseDto<BankResponseDto>.ErrorResponse("Bank not found");

                return ApiResponseDto<BankResponseDto>.SuccessResponse(await MapToResponseDto(bank));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BankResponseDto>.ErrorResponse($"Error retrieving bank: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<BankListResponseDto>>> GetBanksPaginatedAsync(
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (banks, totalCount) = await _bankRepository.GetBanksPaginatedAsync(pagination, filter);

                var bankDtos = new List<BankListResponseDto>();
                foreach (var bank in banks)
                {
                    bankDtos.Add(new BankListResponseDto
                    {
                        BankId = bank.BankId,
                        BankName = bank.BankName,
                        IFSCCode = bank.IFSCCode,
                        ContactNumber = bank.ContactNumber,
                        IsActive = true,
                        TotalClients = bank.Clients?.Count ?? 0,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                var paginatedResponse = new PaginatedResponseDto<BankListResponseDto>(
                    bankDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<BankListResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<BankListResponseDto>>.ErrorResponse($"Error retrieving banks: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<BankListResponseDto>>> GetBanksBySuperAdminAsync(int superAdminId)
        {
            try
            {
                var banks = await _bankRepository.GetBanksBySuperAdminAsync(superAdminId);
                var bankDtos = new List<BankListResponseDto>();

                foreach (var bank in banks)
                {
                    bankDtos.Add(new BankListResponseDto
                    {
                        BankId = bank.BankId,
                        BankName = bank.BankName,
                        IFSCCode = bank.IFSCCode,
                        ContactNumber = bank.ContactNumber,
                        IsActive = true,
                        TotalClients = bank.Clients?.Count ?? 0,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                return ApiResponseDto<IEnumerable<BankListResponseDto>>.SuccessResponse(bankDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<BankListResponseDto>>.ErrorResponse($"Error retrieving banks: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<BankResponseDto>> UpdateBankAsync(UpdateBankRequestDto request)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(request.BankId);
                if (bank == null)
                    return ApiResponseDto<BankResponseDto>.ErrorResponse("Bank not found");

                if (!string.IsNullOrEmpty(request.BankName))
                    bank.BankName = request.BankName;

                if (!string.IsNullOrEmpty(request.Address))
                    bank.Address = request.Address;

                if (!string.IsNullOrEmpty(request.ContactNumber))
                    bank.ContactNumber = request.ContactNumber;

                if (!string.IsNullOrEmpty(request.SupportEmail))
                    bank.SupportEmail = request.SupportEmail;

                await _bankRepository.UpdateAsync(bank);

                await _auditLogService.LogUpdateAsync("Bank", bank.BankId, bank.SuperAdminId, "SuperAdmin");

                return ApiResponseDto<BankResponseDto>.SuccessResponse(
                    await MapToResponseDto(bank),
                    "Bank updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<BankResponseDto>.ErrorResponse($"Error updating bank: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteBankAsync(int bankId)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(bankId);
                if (bank == null)
                    return ApiResponseDto<bool>.ErrorResponse("Bank not found");

                var success = await _bankRepository.DeleteAsync(bankId);

                if (success)
                {
                    await _auditLogService.LogDeleteAsync("Bank", bankId, bank.SuperAdminId, "SuperAdmin");
                }

                return ApiResponseDto<bool>.SuccessResponse(success, "Bank deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error deleting bank: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetBankStatisticsAsync(int bankId)
        {
            try
            {
                var stats = await _bankRepository.GetBankStatisticsAsync(bankId);
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> IsBankActiveAsync(int bankId)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(bankId);
                return ApiResponseDto<bool>.SuccessResponse(bank != null);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error checking bank status: {ex.Message}");
            }
        }

        private async Task<BankResponseDto> MapToResponseDto(Bank bank)
        {
            var totalClients = await _bankRepository.GetTotalClientsCountAsync(bank.BankId);
            var totalBankUsers = await _bankRepository.GetTotalBankUsersCountAsync(bank.BankId);

            return new BankResponseDto
            {
                BankId = bank.BankId,
                BankName = bank.BankName,
                IFSCCode = bank.IFSCCode,
                Address = bank.Address,
                ContactNumber = bank.ContactNumber,
                SupportEmail = bank.SupportEmail,
                Website = null,
                CustomerCareNumber = null,
                IsActive = true,
                SuperAdminId = bank.SuperAdminId,
                SuperAdminName = bank.SuperAdmin?.FullName ?? "Unknown",
                TotalBankUsers = totalBankUsers,
                TotalClients = totalClients,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
        }
    }
}