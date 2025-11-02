using Banking_CapStone.DTO.Request.Auth;
using Banking_CapStone.DTO.Response.Auth;
using Banking_CapStone.DTO.Response.Common;

namespace Banking_CapStone.Service
{
    public interface IAuthService
    {
        Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponseDto<UserProfileResponseDto>> RegisterSuperAdminAsync(RegisterSuperAdminRequestDto request);
        Task<ApiResponseDto<UserProfileResponseDto>> RegisterBankUserAsync(RegisterBankUserRequestDto request);
        Task<ApiResponseDto<UserProfileResponseDto>> RegisterClientAsync(RegisterClientRequestDto request);
        Task<ApiResponseDto<bool>> ChangePasswordAsync(int userId, ChangePasswordRequestDto request);
        Task<ApiResponseDto<UserProfileResponseDto>> GetUserProfileAsync(int userId);

        Task<ApiResponseDto<bool>> DeactivateUserAsync(int userId);

        Task<ApiResponseDto<bool>> ActivateUserAsync(int userId);

        Task<bool> ValidateTokenAsync(string token);

        string GenerateJwtToken(int userId, string username, string role);
    }
}
