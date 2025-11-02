using Banking_CapStone.DTO.Request.Account;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Account;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IAuditLogService _auditLogService;

        public AccountService(
            IAccountRepository accountRepository,
            IClientRepository clientRepository,
            IBankRepository bankRepository,
            IAuditLogService auditLogService)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _bankRepository = bankRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<AccountResponseDto>> CreateAccountAsync(CreateAccountRequestDto request)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Client not found");

                var bank = await _bankRepository.GetByIdAsync(request.BankId);
                if (bank == null)
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Bank not found");

                var accountType = await _accountRepository.GetAccountTypeByIdAsync(request.AccountTypeId);
                if (accountType == null)
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Invalid account type");

                string accountNumber = string.IsNullOrEmpty(request.AccountNumber)
                    ? GenerateAccountNumber()
                    : request.AccountNumber;

                if (await _accountRepository.IsAccountNumberExistsAsync(accountNumber))
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Account number already exists");

                var account = new Account
                {
                    AccountNumber = accountNumber,
                    ClientId = request.ClientId,
                    BankId = request.BankId,
                    Balance = request.InitialBalance,
                    AccountTypeId = request.AccountTypeId,
                    AccountStatusId = request.ActivateImmediately ? 1 : 2,
                    CreatedAt = DateTime.UtcNow
                };

                await _accountRepository.AddAsync(account);

                await _auditLogService.LogCreateAsync("Account", account.AccountId, request.CreatedByBankUserId, "BankUser");

                return ApiResponseDto<AccountResponseDto>.SuccessResponse(
                    await MapToResponseDto(account),
                    "Account created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AccountResponseDto>.ErrorResponse($"Error creating account: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AccountResponseDto>> GetAccountByIdAsync(int accountId)
        {
            try
            {
                var account = await _accountRepository.GetAccountWithDetailsAsync(accountId);
                if (account == null)
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Account not found");

                return ApiResponseDto<AccountResponseDto>.SuccessResponse(await MapToResponseDto(account));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AccountResponseDto>.ErrorResponse($"Error retrieving account: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AccountResponseDto>> GetAccountByNumberAsync(string accountNumber)
        {
            try
            {
                var account = await _accountRepository.GetAccountByNumberAsync(accountNumber);
                if (account == null)
                    return ApiResponseDto<AccountResponseDto>.ErrorResponse("Account not found");

                return ApiResponseDto<AccountResponseDto>.SuccessResponse(await MapToResponseDto(account));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AccountResponseDto>.ErrorResponse($"Error retrieving account: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AccountBalanceResponseDto>> GetAccountBalanceAsync(int accountId)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                    return ApiResponseDto<AccountBalanceResponseDto>.ErrorResponse("Account not found");

                var balanceDto = new AccountBalanceResponseDto
                {
                    AccountId = account.AccountId,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    AvailableBalance = account.Balance,
                    HoldAmount = 0,
                    Currency = "INR",
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponseDto<AccountBalanceResponseDto>.SuccessResponse(balanceDto);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AccountBalanceResponseDto>.ErrorResponse($"Error retrieving balance: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<AccountResponseDto>>> GetAccountsByClientIdAsync(int clientId)
        {
            try
            {
                var accounts = await _accountRepository.GetAccountsByClientIdAsync(clientId);
                var accountDtos = new List<AccountResponseDto>();

                foreach (var account in accounts)
                {
                    accountDtos.Add(await MapToResponseDto(account));
                }

                return ApiResponseDto<IEnumerable<AccountResponseDto>>.SuccessResponse(accountDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AccountResponseDto>>.ErrorResponse($"Error retrieving accounts: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<AccountResponseDto>>> GetActiveAccountsAsync(int clientId)
        {
            try
            {
                var accounts = await _accountRepository.GetActiveAccountsAsync(clientId);
                var accountDtos = new List<AccountResponseDto>();

                foreach (var account in accounts)
                {
                    accountDtos.Add(await MapToResponseDto(account));
                }

                return ApiResponseDto<IEnumerable<AccountResponseDto>>.SuccessResponse(accountDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<AccountResponseDto>>.ErrorResponse($"Error retrieving accounts: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<AccountResponseDto>>> GetAccountsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (accounts, totalCount) = await _accountRepository.GetAccountsPaginatedAsync(
                    clientId, bankId, pagination, filter);

                var accountDtos = new List<AccountResponseDto>();
                foreach (var account in accounts)
                {
                    accountDtos.Add(await MapToResponseDto(account));
                }

                var paginatedResponse = new PaginatedResponseDto<AccountResponseDto>(
                    accountDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<AccountResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<AccountResponseDto>>.ErrorResponse($"Error retrieving accounts: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> UpdateAccountStatusAsync(UpdateAccountStatusRequestDto request)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(request.AccountId);
                if (account == null)
                    return ApiResponseDto<bool>.ErrorResponse("Account not found");

                var success = await _accountRepository.UpdateAccountStatusAsync(
                    request.AccountId,
                    request.AccountStatusId,
                    request.ReasonForChange,
                    request.UpdatedByBankUserId);

                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Account", request.AccountId, request.UpdatedByBankUserId, "BankUser",
                        $"Status changed to {request.AccountStatusId}. Reason: {request.ReasonForChange}");
                }

                return ApiResponseDto<bool>.SuccessResponse(success, "Account status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error updating account status: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> CreditAccountAsync(int accountId, decimal amount, string description)
        {
            try
            {
                if (amount <= 0)
                    return ApiResponseDto<bool>.ErrorResponse("Amount must be positive");

                var success = await _accountRepository.CreditAccountAsync(accountId, amount);
                return ApiResponseDto<bool>.SuccessResponse(success, "Account credited successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error crediting account: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DebitAccountAsync(int accountId, decimal amount, string description)
        {
            try
            {
                if (amount <= 0)
                    return ApiResponseDto<bool>.ErrorResponse("Amount must be positive");

                var account = await _accountRepository.GetByIdAsync(accountId);
                if (account == null)
                    return ApiResponseDto<bool>.ErrorResponse("Account not found");

                if (account.Balance < amount)
                    return ApiResponseDto<bool>.ErrorResponse("Insufficient balance");

                var success = await _accountRepository.DebitAccountAsync(accountId, amount);
                return ApiResponseDto<bool>.SuccessResponse(success, "Account debited successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error debiting account: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalBalanceByClientAsync(int clientId)
        {
            try
            {
                var totalBalance = await _accountRepository.GetTotalBalanceByClientIdAsync(clientId);
                return ApiResponseDto<decimal>.SuccessResponse(totalBalance);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating total balance: {ex.Message}");
            }
        }

        public string GenerateAccountNumber()
        {
            var random = new Random();
            var digits = random.Next(10000000, 99999999);
            var alphanumeric = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            return $"BPA{digits}{alphanumeric}";
        }

        private async Task<AccountResponseDto> MapToResponseDto(Account account)
        {
            var client = account.Client ?? await _clientRepository.GetByIdAsync(account.ClientId);
            var bank = account.Bank ?? await _bankRepository.GetByIdAsync(account.BankId);
            var accountType = account.AccountType ?? await _accountRepository.GetAccountTypeByIdAsync(account.AccountTypeId);
            var accountStatus = account.AccountStatus ?? await _accountRepository.GetAccountStatusByIdAsync(account.AccountStatusId);

            return new AccountResponseDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                ClientId = account.ClientId,
                ClientName = client?.ClientName ?? "Unknown",
                BankId = account.BankId,
                BankName = bank?.BankName ?? "Unknown",
                Balance = account.Balance,
                AccountType = accountType?.Type.ToString() ?? "Unknown",
                AccountStatus = accountStatus?.Status.ToString() ?? "Unknown",
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt,
            };
        }
    }
}