using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Employee;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Employee;

namespace Banking_CapStone.Service
{
    public interface IEmployeeService
    {
        Task<ApiResponseDto<EmployeeResponseDto>> CreateEmployeeAsync(CreateEmployeeRequestDto request);
        Task<ApiResponseDto<EmployeeResponseDto>> GetEmployeeByIdAsync(int employeeId);
        Task<ApiResponseDto<EmployeeResponseDto>> GetEmployeeByEmailAsync(string email);
        Task<ApiResponseDto<IEnumerable<EmployeeListResponseDto>>> GetEmployeesByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<EmployeeListResponseDto>>> GetActiveEmployeesAsync(int clientId);
        Task<ApiResponseDto<PaginatedResponseDto<EmployeeListResponseDto>>> GetEmployeesPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<EmployeeResponseDto>> UpdateEmployeeAsync(UpdateEmployeeRequestDto request);
        Task<ApiResponseDto<bool>> DeactivateEmployeeAsync(int employeeId, string reason);
        Task<ApiResponseDto<bool>> ActivateEmployeeAsync(int employeeId);
        Task<ApiResponseDto<bool>> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary, string reason);
        Task<ApiResponseDto<decimal>> GetTotalMonthlySalaryBurdenAsync(int clientId);
        Task<ApiResponseDto<int>> GetActiveEmployeesCountAsync(int clientId);
    }
}
