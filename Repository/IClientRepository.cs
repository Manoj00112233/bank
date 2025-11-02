using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<Client?> GetClientWithDetailsAsync(int clientId);
        Task<Client?> GetClientWithAccountsAsync(int clientId);
        Task<Client?> GetClientWithEmployeesAsync(int clientId);
        Task<Client?> GetClientWithBeneficiariesAsync(int clientId);
        Task<Client?> GetClientByUsernameAsync(string username);
        Task<Client?> GetClientByPanAsync(string panNumber);
        Task<IEnumerable<Client>> GetClientsByBankIdAsync(int bankId);
        Task<IEnumerable<Client>> GetActiveClientsAsync(int bankId);
        Task<(IEnumerable<Client> Clients, int TotalCount)> GetClientsPaginatedAsync(int bankId, PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> IsPanExistsAsync(string panNumber);
        Task<bool> IsGstExistsAsync(string gstNumber);
        Task<bool> VerifyClientAsync(int clientId, int verifiedByBankUserId);
        Task<decimal> GetClientBalanceAsync(int clientId);
        Task<bool> UpdateClientBalanceAsync(int clientId, decimal newBalance);
        Task<int> GetTotalEmployeesCountAsync(int clientId);
        Task<int> GetTotalBeneficiariesCountAsync(int clientId);

        Task<Dictionary<string,object>> GetClientStatisticsAsync (int clientId);
    }
}
