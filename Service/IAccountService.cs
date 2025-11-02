using Banking_CapStone.DTO.Request.Account;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Account;
using Banking_CapStone.DTO.Response.Common;

namespace Banking_CapStone.Service
{
    public interface IAccountService
    {
        Task<ApiResponseDto<AccountResponseDto>> CreateAccountAsync(CreateAccountRequestDto request);
        Task<ApiResponseDto<AccountResponseDto>> GetAccountByIdAsync(int accountId);
        Task<ApiResponseDto<AccountResponseDto>> GetAccountByNumberAsync(string accountNumber);
        Task<ApiResponseDto<AccountBalanceResponseDto>> GetAccountBalanceAsync(int accountId);
        Task<ApiResponseDto<IEnumerable<AccountResponseDto>>> GetAccountsByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<AccountResponseDto>>> GetActiveAccountsAsync(int clientId);
        Task<ApiResponseDto<PaginatedResponseDto<AccountResponseDto>>> GetAccountsPaginatedAsync(
            int? clientId,
            int? bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);

        Task<ApiResponseDto<bool>> UpdateAccountStatusAsync(UpdateAccountStatusRequestDto request);
        Task<ApiResponseDto<bool>> CreditAccountAsync(int accountId, decimal amount, string description);
        Task<ApiResponseDto<bool>> DebitAccountAsync(int accountId, decimal amount, string description);
        Task<ApiResponseDto<decimal>> GetTotalBalanceByClientAsync(int clientId);
        string GenerateAccountNumber();
    }
}
