using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BankingDbContext context) : base(context)
        {
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByClientIdAsync(int clientId)
        {
            return await _context.Employees
                .Where(e=> e.ClientId == clientId)
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync(int clientId)
        {
            return await _context.Employees
                .Where(e => e.ClientId == clientId && e.IsActive)
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetInactiveEmployeesAsync(int clientId)
        {
            return await _context.Employees
                .Where(e => e.ClientId == clientId && !e.IsActive)
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            var query = _context.Employees
                .Where(e => e.ClientId == clientId)
                .Include(e => e.Client)
                .AsQueryable();

            // Apply filters
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(e =>
                        e.FullName.Contains(filter.SearchTerm) ||
                        e.Email.Contains(filter.SearchTerm));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(e => e.IsActive == filter.IsActive.Value);
                }
            }

            var totalCount = await query.CountAsync();

            // Apply pagination
            var employees = await query
                .OrderBy(e => e.FullName)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (employees, totalCount);
        }


        public async Task<bool> IsEmployeeEmailExistsAsync(string email, int clientId)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && e.ClientId == clientId);
        }

        public async Task<bool> IsEmployeeCodeExistsAsync(string employeeCode, int clientId)
        {
            return await _context.Employees
                .AnyAsync(e => e.EmployeeId.ToString() == employeeCode && e.ClientId == clientId);
        }

        public async Task<bool> DeactivateEmployeeAsync(int employeeId)
        {
            var employee = await GetByIdAsync(employeeId);
            if (employee == null) return false;

            employee.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateEmployeeAsync(int employeeId)
        {
            var employee = await GetByIdAsync(employeeId);
            if (employee == null) return false;

            employee.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetEmployeeSalaryAsync(int employeeId)
        {
            var employee = await GetByIdAsync(employeeId);
            return employee?.Salary ?? 0;
        }

        public async Task<bool> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary)
        {
            var employee = await GetByIdAsync(employeeId);
            if (employee == null) return false;

            employee.Salary = newSalary;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int clientId, string department)
        {
            return await _context.Employees
                .Where(e => e.ClientId == clientId && e.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDesignationAsync(int clientId, string designation)
        {
            return await _context.Employees
                .Where(e => e.ClientId == clientId && e.IsActive)
                .ToListAsync();
        }

        public async Task<int> GetTotalActiveEmployeesCountAsync(int clientId)
        {
            return await _context.Employees
                .CountAsync(e => e.ClientId == clientId && e.IsActive);
        }

        public async Task<decimal> GetTotalMonthlySalaryBurdenAsync(int clientId)
        {
            return await _context.Employees
                .Where(e => e.ClientId == clientId && e.IsActive)
                .SumAsync(e => e.Salary);
        }
    }
}
