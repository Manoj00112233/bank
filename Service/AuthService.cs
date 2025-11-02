using Banking_CapStone.DTO.Request.Auth;
using Banking_CapStone.DTO.Response.Auth;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Banking_CapStone.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IAuthRepository authRepository,
            IAuditLogService auditLogService,
            IConfiguration configuration,
            IPasswordHasher passwordHasher)
        {
            _authRepository = authRepository;
            _auditLogService = auditLogService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var user = await _authRepository.GetUserByUsernameAsync(request.Username);
                if (user == null)
                    return ApiResponseDto<LoginResponseDto>.ErrorResponse("Invalid username or password");

                
                if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
                    return ApiResponseDto<LoginResponseDto>.ErrorResponse("Invalid username or password");

                if (!user.IsActive)
                    return ApiResponseDto<LoginResponseDto>.ErrorResponse("Account is deactivated");

                var role = user.UserRole?.Role.ToString() ?? "Unknown";
                var token = GenerateJwtToken(user.Id, user.Username, role);

                await _auditLogService.LogLoginAsync(user.Id, user.Username, true);

                var userProfile = await MapToUserProfileDto(user);

                var loginResponse = new LoginResponseDto
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddHours(
                        Convert.ToDouble(_configuration["Jwt:DurationHours"] ?? "24")),
                    User = userProfile,
                    Message = "Login Successful"
                };

                return ApiResponseDto<LoginResponseDto>.SuccessResponse(loginResponse, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<LoginResponseDto>.ErrorResponse($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<UserProfileResponseDto>> RegisterSuperAdminAsync(RegisterSuperAdminRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Email is required");
            try
            {
                if (await _authRepository.IsUsernameExistsAsync(request.Username))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Username already exists");

                if (await _authRepository.IsEmailExistsAsync(request.Email))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Email already exists");

                var superAdmin = new SuperAdmin
                {
                    FullName = request.FullName,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    RoleId = 1,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdAdmin = await _authRepository.CreateSuperAdminAsync(superAdmin);
                await _auditLogService.LogCreateAsync("SuperAdmin", createdAdmin.Id, createdAdmin.Id, createdAdmin.Username);

                return ApiResponseDto<UserProfileResponseDto>.SuccessResponse(
                    await MapToUserProfileDto(createdAdmin),
                    "Super Admin registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<UserProfileResponseDto>.ErrorResponse($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<UserProfileResponseDto>> RegisterBankUserAsync(RegisterBankUserRequestDto request)
        {
            try
            {
                if (await _authRepository.IsUsernameExistsAsync(request.Username))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Username already exists");

                if (await _authRepository.IsEmailExistsAsync(request.Email))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Email already exists");

                var bankUser = new BankUser
                {
                    FullName = request.FullName,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    BankId = request.BankId,
                    RoleId = 2,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _authRepository.CreateBankUserAsync(bankUser);
                await _auditLogService.LogCreateAsync("BankUser", createdUser.Id, createdUser.Id, createdUser.Username);

                return ApiResponseDto<UserProfileResponseDto>.SuccessResponse(
                    await MapToUserProfileDto(createdUser),
                    "Bank User registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<UserProfileResponseDto>.ErrorResponse($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<UserProfileResponseDto>> RegisterClientAsync(RegisterClientRequestDto request)
        {
            try
            {
                if (await _authRepository.IsUsernameExistsAsync(request.Username))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Username already exists");

                if (await _authRepository.IsEmailExistsAsync(request.Email))
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("Email already exists");

                var accountNumber = GenerateAccountNumber();

                var client = new Client
                {
                    ClientName = request.ClientName,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    AccountNumber = accountNumber,
                    AccountBalance = request.InitialBalance,
                    BankId = request.BankId,
                    RoleId = 3,
                    IsActive = true
                };

                var createdClient = await _authRepository.CreateClientAsync(client);
                await _auditLogService.LogCreateAsync("Client", createdClient.Id, request.OnboardedByBankUserId, "BankUser");

                return ApiResponseDto<UserProfileResponseDto>.SuccessResponse(
                    await MapToUserProfileDto(createdClient),
                    "Client registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<UserProfileResponseDto>.ErrorResponse($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ChangePasswordAsync(int userId, ChangePasswordRequestDto request)
        {
            try
            {
                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return ApiResponseDto<bool>.ErrorResponse("User not found");

                if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.CurrentPassword))
                    return ApiResponseDto<bool>.ErrorResponse("Current password is incorrect");

                var newPasswordHash = HashPassword(request.NewPassword);
                var success = await _authRepository.UpdatePasswordAsync(userId, newPasswordHash);

                if (success)
                    await _auditLogService.LogUpdateAsync("User", userId, userId, user.Username, "Password changed");

                return ApiResponseDto<bool>.SuccessResponse(success, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Password change failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<UserProfileResponseDto>> GetUserProfileAsync(int userId)
        {
            try
            {
                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return ApiResponseDto<UserProfileResponseDto>.ErrorResponse("User not found");

                return ApiResponseDto<UserProfileResponseDto>.SuccessResponse(await MapToUserProfileDto(user));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<UserProfileResponseDto>.ErrorResponse($"Error retrieving profile: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeactivateUserAsync(int userId)
        {
            try
            {
                var success = await _authRepository.DeactivateUserAsync(userId);
                if (success)
                    await _auditLogService.LogUpdateAsync("User", userId, userId, "System", "User deactivated");

                return ApiResponseDto<bool>.SuccessResponse(success, "User deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Deactivation failed: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ActivateUserAsync(int userId)
        {
            try
            {
                var success = await _authRepository.ActivateUserAsync(userId);
                if (success)
                    await _auditLogService.LogUpdateAsync("User", userId, userId, "System", "User activated");

                return ApiResponseDto<bool>.SuccessResponse(success, "User activated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Activation failed: {ex.Message}");
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GenerateJwtToken(int userId, string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:DurationHours"] ?? "24")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

        private async Task<UserProfileResponseDto> MapToUserProfileDto(UserBase user)
        {
            var profile = new UserProfileResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.UserRole?.Role.ToString() ?? "Unknown",
                RoleId = user.RoleId,
                IsActive = user.IsActive
            };

            if (user is SuperAdmin superAdmin)
            {
                profile.FullName = superAdmin.FullName;
            }
            else if (user is BankUser bankUser)
            {
                profile.FullName = bankUser.FullName;
                profile.BankId = bankUser.BankId;
                profile.BankName = bankUser.Bank?.BankName;
            }
            else if (user is Client client)
            {
                profile.FullName = client.ClientName;
                profile.ClientId = client.ClientId;
                profile.ClientName = client.ClientName;
                profile.BankId = client.BankId;
                profile.BankName = client.Bank?.BankName;
            }

            return profile;
        }
    }
}
