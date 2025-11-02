using Banking_CapStone.DTO.Request.Client;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Client;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;
using System.Security.Cryptography;
using System.Text;

namespace Banking_CapStone.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuditLogService _auditLogService;

        public ClientService(
            IClientRepository clientRepository,
            IBankRepository bankRepository,
            IAccountRepository accountRepository,
            IAuditLogService auditLogService)
        {
            _clientRepository = clientRepository;
            _bankRepository = bankRepository;
            _accountRepository = accountRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<ClientResponseDto>> OnboardClientAsync(ClientOnboardingRequestDto request)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(request.BankId);
                if (bank == null)
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("Bank not found");

                if (await _clientRepository.IsPanExistsAsync(request.PanNumber))
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("PAN already exists");

                var accountNumber = string.IsNullOrEmpty(request.PreferredAccountNumber)
                    ? GenerateAccountNumber()
                    : request.PreferredAccountNumber;

                if (await _accountRepository.IsAccountNumberExistsAsync(accountNumber))
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("Account number already exists");

                var client = new Client
                {
                    ClientName = request.ClientName,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    //Phone = request.Phone,
                    Address = request.Address,
                    PanNumber = request.PanNumber,
                    AccountNumber = accountNumber,
                    AccountBalance = request.InitialBalance,
                    BankId = request.BankId,
                    RoleId = 3,
                    IsActive = request.ActivateImmediately
                };

                await _clientRepository.AddAsync(client);

                var account = new Account
                {
                    AccountNumber = accountNumber,
                    ClientId = client.ClientId,
                    BankId = request.BankId,
                    Balance = request.InitialBalance,
                    AccountTypeId = request.AccountTypeId,
                    AccountStatusId = request.ActivateImmediately ? 1 : 2,
                    CreatedAt = DateTime.UtcNow
                };

                await _accountRepository.AddAsync(account);

                await _auditLogService.LogCreateAsync("Client", client.ClientId, request.OnboardedByBankUserId, "BankUser");

                return ApiResponseDto<ClientResponseDto>.SuccessResponse(
                    MapToClientResponseDto(client),
                    "Client onboarded successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientResponseDto>.ErrorResponse($"Error onboarding client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientResponseDto>> CreateClientAsync(CreateClientRequestDto request)
        {
            try
            {
                var bank = await _bankRepository.GetByIdAsync(request.BankId);
                if (bank == null)
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("Bank not found");

                if (await _clientRepository.IsPanExistsAsync(request.PanNumber))
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("PAN already exists");

                var accountNumber = GenerateAccountNumber();

                var client = new Client
                {
                    ClientName = request.ClientName,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                   // Phone = request.Phone.ToString(),
                    Address = request.Address,
                    PanNumber = request.PanNumber,
                    AccountNumber = accountNumber,
                    AccountBalance = 0,
                    BankId = request.BankId,
                    RoleId = 3,
                    IsActive = true
                };

                await _clientRepository.AddAsync(client);

                await _auditLogService.LogCreateAsync("Client", client.ClientId, request.OnboardedByBankUserId, "BankUser");

                return ApiResponseDto<ClientResponseDto>.SuccessResponse(
                    MapToClientResponseDto(client),
                    "Client created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientResponseDto>.ErrorResponse($"Error creating client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientByIdAsync(int clientId)
        {
            try
            {
                var client = await _clientRepository.GetClientWithDetailsAsync(clientId);
                if (client == null)
                    return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse("Client not found");

                return ApiResponseDto<ClientDetailsResponseDto>.SuccessResponse(await MapToClientDetailsDto(client));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse($"Error retrieving client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithAccountsAsync(int clientId)
        {
            try
            {
                var client = await _clientRepository.GetClientWithAccountsAsync(clientId);
                if (client == null)
                    return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse("Client not found");

                return ApiResponseDto<ClientDetailsResponseDto>.SuccessResponse(await MapToClientDetailsDto(client));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse($"Error retrieving client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithEmployeesAsync(int clientId)
        {
            try
            {
                var client = await _clientRepository.GetClientWithEmployeesAsync(clientId);
                if (client == null)
                    return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse("Client not found");

                return ApiResponseDto<ClientDetailsResponseDto>.SuccessResponse(await MapToClientDetailsDto(client));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse($"Error retrieving client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithBeneficiariesAsync(int clientId)
        {
            try
            {
                var client = await _clientRepository.GetClientWithBeneficiariesAsync(clientId);
                if (client == null)
                    return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse("Client not found");

                return ApiResponseDto<ClientDetailsResponseDto>.SuccessResponse(await MapToClientDetailsDto(client));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientDetailsResponseDto>.ErrorResponse($"Error retrieving client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<ClientResponseDto>>> GetClientsPaginatedAsync(
            int bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (clients, totalCount) = await _clientRepository.GetClientsPaginatedAsync(bankId, pagination, filter);

                var clientDtos = clients.Select(MapToClientResponseDto).ToList();

                var paginatedResponse = new PaginatedResponseDto<ClientResponseDto>(
                    clientDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<ClientResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<ClientResponseDto>>.ErrorResponse($"Error retrieving clients: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<ClientResponseDto>> UpdateClientAsync(UpdateClientRequestDto request)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                    return ApiResponseDto<ClientResponseDto>.ErrorResponse("Client not found");

                if (!string.IsNullOrEmpty(request.ClientName))
                    client.ClientName = request.ClientName;

                if (!string.IsNullOrEmpty(request.Email))
                    client.Email = request.Email;

                //if (!string.IsNullOrEmpty(request.Phone))
                //    client.Phone = request.Phone;

                await _clientRepository.UpdateAsync(client);

                await _auditLogService.LogUpdateAsync("Client", client.ClientId, 0, "System");

                return ApiResponseDto<ClientResponseDto>.SuccessResponse(
                    MapToClientResponseDto(client),
                    "Client updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<ClientResponseDto>.ErrorResponse($"Error updating client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> VerifyClientAsync(int clientId, int verifiedByBankUserId)
        {
            try
            {
                var success = await _clientRepository.VerifyClientAsync(clientId, verifiedByBankUserId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Client", clientId, verifiedByBankUserId, "BankUser", "Client verified");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Client verified successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error verifying client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeactivateClientAsync(int clientId)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return ApiResponseDto<bool>.ErrorResponse("Client not found");

                client.IsActive = false;
                await _clientRepository.UpdateAsync(client);

                await _auditLogService.LogUpdateAsync("Client", clientId, 0, "System", "Client deactivated");

                return ApiResponseDto<bool>.SuccessResponse(true, "Client deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error deactivating client: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<Dictionary<string, object>>> GetClientStatisticsAsync(int clientId)
        {
            try
            {
                var stats = await _clientRepository.GetClientStatisticsAsync(clientId);
                return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
            }
        }

        private ClientResponseDto MapToClientResponseDto(Client client)
        {
            return new ClientResponseDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Email = client.Email,
                //Phone = client.Phone,
                Username = client.Username,
                BankId = client.BankId,
                BankName = client.Bank?.BankName ?? "Unknown",
                IsActive = client.IsActive,
                CreatedAt = DateTime.UtcNow
            };
        }

        private async Task<ClientDetailsResponseDto> MapToClientDetailsDto(Client client)
        {
            var totalBalance = await _clientRepository.GetClientBalanceAsync(client.ClientId);
            var totalEmployees = await _clientRepository.GetTotalEmployeesCountAsync(client.ClientId);
            var totalBeneficiaries = await _clientRepository.GetTotalBeneficiariesCountAsync(client.ClientId);

            var accounts = client.Accounts?.Select(a => new AccountSummaryDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType?.Type.ToString() ?? "Unknown",
                AccountStatus = a.AccountStatus?.Status.ToString() ?? "Unknown",
                Balance = a.Balance
            }).ToList() ?? new List<AccountSummaryDto>();

            return new ClientDetailsResponseDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Email = client.Email,
                //Phone = client.Phone,
                Address = client.Address,
                PanNumber = client.PanNumber,
                GstNumber = client.GstNumber,
                DateOfIncorporation = DateTime.UtcNow,
                BankId = client.BankId,
                BankName = client.Bank?.BankName ?? "Unknown",
                TotalBalance = totalBalance,
                TotalAccounts = accounts.Count,
                TotalEmployees = totalEmployees,
                TotalBeneficiaries = totalBeneficiaries,
                IsActive = client.IsActive,
                CreatedAt = DateTime.UtcNow,
                Accounts = accounts
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            var digits = random.Next(10000000, 99999999);
            var alphanumeric = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            return $"BPA{digits}{alphanumeric}";
        }
    }
}