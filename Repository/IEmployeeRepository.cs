using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int employeeId);
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetEmployeesByClientIdAsync(int clientId);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync(int clientId);
        Task<IEnumerable<Employee>> GetInactiveEmployeesAsync(int clientId);
        Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesPaginatedAsync(int clientId, PaginationRequestDto pagination, FilterRequestDto? filter = null);
        Task<bool> IsEmployeeEmailExistsAsync(string email, int clientId);
        Task<bool> IsEmployeeCodeExistsAsync(string employeeCode, int clientId);
        Task<bool> DeactivateEmployeeAsync(int employeeId);
        Task<bool> ActivateEmployeeAsync(int employeeId);
        Task<decimal> GetEmployeeSalaryAsync(int employeeId);
        Task<bool> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int clientId, string department);
        Task<IEnumerable<Employee>> GetEmployeesByDesignationAsync(int clientId, string designation);
        Task<int> GetTotalActiveEmployeesCountAsync(int clientId);
        Task<decimal> GetTotalMonthlySalaryBurdenAsync(int clientId);   


    }
}
