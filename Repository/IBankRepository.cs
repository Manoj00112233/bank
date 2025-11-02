using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IBankRepository : IBaseRepository<Bank>
    {
        Task<Bank?> GetBankWithDetailsAsync(int bankId);
        Task<Bank?> GetBankWithClientsAsync(int bankId);
        Task<Bank?> GetBankWithBankUsersAsync(int bankId);
        Task<Bank?> GetBankByIFSCAsync(string ifscCode);
        Task<IEnumerable<Bank>> GetActiveBanksAsync();
        Task<IEnumerable<Bank>> GetBanksBySuperAdminAsync(int superAdminId);
        Task<(IEnumerable<Bank> Banks, int TotalCount)> GetBanksPaginatedAsync(PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> IsIFSCExistsAsync(string ifscCode);

        Task<int> GetTotalClientsCountAsync(int bankId);
        Task<int> GetTotalBankUsersCountAsync(int bankId);
        Task<decimal> GetTotalBankBalanceAsync (int bankId);

        Task<Dictionary<string,object>> GetBankStatisticsAsync (int bankId);
    }
}
