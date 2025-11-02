using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IAuthRepository
    {
        Task<UserBase?> GetUserByUsernameAsync (string username);
        Task<UserBase?> GetUserByEmailAsync (string email);
        Task<UserBase?> GetUserByIdAsync(int userId);
        Task<bool> ValidateCredentialsAsync(string username, string passwordHash);
        Task<SuperAdmin> CreateSuperAdminAsync(SuperAdmin superAdmin);
        Task<BankUser> CreateBankUserAsync(BankUser bankUser);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        Task<UserRole?> GetUserRoleByIdAsync(int roleId);
        Task<IEnumerable<UserBase>> GetUsersByRoleAsync(int roleId);
    }
}
