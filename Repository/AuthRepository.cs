using Banking_CapStone.Data;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BankingDbContext _context;

        public AuthRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<UserBase?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserBase?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserBase?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string passwordHash)
        {
            var user = await GetUserByUsernameAsync(username);
            return user != null && user.PasswordHash == passwordHash && user.IsActive;
        }

        public async Task<SuperAdmin> CreateSuperAdminAsync(SuperAdmin superAdmin)
        {
            superAdmin.RoleId = 1; // SUPER_ADMIN role
            superAdmin.CreatedAt = DateTime.UtcNow;
            await _context.SuperAdmins.AddAsync(superAdmin);
            await _context.SaveChangesAsync();
            return superAdmin;
        }

        public async Task<BankUser> CreateBankUserAsync(BankUser bankUser)
        {
            bankUser.RoleId = 2; // BANK_USER role
            bankUser.CreatedAt = DateTime.UtcNow;
            await _context.BankUsers.AddAsync(bankUser);
            await _context.SaveChangesAsync();
            return bankUser;
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            client.RoleId = 3; // CLIENT_USER role
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.PasswordHash = newPasswordHash;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserRole> GetUserRoleByIdAsync(int roleId)
        {
            return await _context.UserRoles.FindAsync(roleId);
        }

        public async Task<IEnumerable<UserBase>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.Users
                .Where(u=> u.RoleId == roleId)
                .ToListAsync();
        }
    }
}
