using Banking_CapStone.DTO.Request.Client;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Response.Client;
using Banking_CapStone.DTO.Response.Common;

namespace Banking_CapStone.Service
{
    public interface IClientService
    {
        Task<ApiResponseDto<ClientResponseDto>> OnboardClientAsync(ClientOnboardingRequestDto request);
        Task<ApiResponseDto<ClientResponseDto>> CreateClientAsync(CreateClientRequestDto request);
        Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientByIdAsync(int clientId);
        Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithAccountsAsync(int clientId);
        Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithEmployeesAsync(int clientId);
        Task<ApiResponseDto<ClientDetailsResponseDto>> GetClientWithBeneficiariesAsync(int clientId);

        Task<ApiResponseDto<PaginatedResponseDto<ClientResponseDto>>> GetClientsPaginatedAsync(
            int bankId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<ClientResponseDto>> UpdateClientAsync(UpdateClientRequestDto request);
        Task<ApiResponseDto<bool>> VerifyClientAsync(int clientId, int verifiedByBankUserId);
        Task<ApiResponseDto<bool>> DeactivateClientAsync(int clientId);
        Task<ApiResponseDto<Dictionary<string, object>>> GetClientStatisticsAsync(int clientId);
    }
}
