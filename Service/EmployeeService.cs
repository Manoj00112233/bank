using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Employee;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Employee;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;

namespace Banking_CapStone.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAuditLogService _auditLogService;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IClientRepository clientRepository,
            IAuditLogService auditLogService)
        {
            _employeeRepository = employeeRepository;
            _clientRepository = clientRepository;
            _auditLogService = auditLogService;
        }

        public async Task<ApiResponseDto<EmployeeResponseDto>> CreateEmployeeAsync(CreateEmployeeRequestDto request)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Client not found");

                if (await _employeeRepository.IsEmployeeEmailExistsAsync(request.Email, request.ClientId))
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Email already exists");

                var employee = new Employee
                {
                    FullName = request.EmployeeName,
                    Email = request.Email,
                    Salary = request.Salary,
                    ClientId = request.ClientId,
                    IsActive = request.IsActive
                };

                await _employeeRepository.AddAsync(employee);

                await _auditLogService.LogCreateAsync("Employee", employee.EmployeeId, request.ClientId, "Client");

                return ApiResponseDto<EmployeeResponseDto>.SuccessResponse(
                    await MapToResponseDto(employee),
                    "Employee created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<EmployeeResponseDto>.ErrorResponse($"Error creating employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<EmployeeResponseDto>> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeWithDetailsAsync(employeeId);
                if (employee == null)
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Employee not found");

                return ApiResponseDto<EmployeeResponseDto>.SuccessResponse(await MapToResponseDto(employee));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<EmployeeResponseDto>.ErrorResponse($"Error retrieving employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<EmployeeResponseDto>> GetEmployeeByEmailAsync(string email)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByEmailAsync(email);
                if (employee == null)
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Employee not found");

                return ApiResponseDto<EmployeeResponseDto>.SuccessResponse(await MapToResponseDto(employee));
            }
            catch (Exception ex)
            {
                return ApiResponseDto<EmployeeResponseDto>.ErrorResponse($"Error retrieving employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<EmployeeListResponseDto>>> GetEmployeesByClientIdAsync(int clientId)
        {
            try
            {
                var employees = await _employeeRepository.GetEmployeesByClientIdAsync(clientId);
                var employeeDtos = employees.Select(e => new EmployeeListResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.FullName,
                    Email = e.Email,
                    EmployeeCode = e.EmployeeId.ToString(),
                    Designation = null,
                    Salary = e.Salary,
                    IsActive = e.IsActive,
                    JoiningDate = DateTime.UtcNow
                }).ToList();

                return ApiResponseDto<IEnumerable<EmployeeListResponseDto>>.SuccessResponse(employeeDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<EmployeeListResponseDto>>.ErrorResponse($"Error retrieving employees: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<IEnumerable<EmployeeListResponseDto>>> GetActiveEmployeesAsync(int clientId)
        {
            try
            {
                var employees = await _employeeRepository.GetActiveEmployeesAsync(clientId);
                var employeeDtos = employees.Select(e => new EmployeeListResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.FullName,
                    Email = e.Email,
                    EmployeeCode = e.EmployeeId.ToString(),
                    Designation = null,
                    Salary = e.Salary,
                    IsActive = e.IsActive,
                    JoiningDate = DateTime.UtcNow
                }).ToList();

                return ApiResponseDto<IEnumerable<EmployeeListResponseDto>>.SuccessResponse(employeeDtos);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<IEnumerable<EmployeeListResponseDto>>.ErrorResponse($"Error retrieving employees: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<PaginatedResponseDto<EmployeeListResponseDto>>> GetEmployeesPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null)
        {
            try
            {
                var (employees, totalCount) = await _employeeRepository.GetEmployeesPaginatedAsync(clientId, pagination, filter);

                var employeeDtos = employees.Select(e => new EmployeeListResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.FullName,
                    Email = e.Email,
                    EmployeeCode = e.EmployeeId.ToString(),
                    Designation = null,
                    Salary = e.Salary,
                    IsActive = e.IsActive,
                    JoiningDate = DateTime.UtcNow
                }).ToList();

                var paginatedResponse = new PaginatedResponseDto<EmployeeListResponseDto>(
                    employeeDtos, totalCount, pagination.PageNumber, pagination.PageSize);

                return ApiResponseDto<PaginatedResponseDto<EmployeeListResponseDto>>.SuccessResponse(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<PaginatedResponseDto<EmployeeListResponseDto>>.ErrorResponse($"Error retrieving employees: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<EmployeeResponseDto>> UpdateEmployeeAsync(UpdateEmployeeRequestDto request)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Employee not found");

                if (employee.ClientId != request.ClientId)
                    return ApiResponseDto<EmployeeResponseDto>.ErrorResponse("Unauthorized");

                if (!string.IsNullOrEmpty(request.EmployeeName))
                    employee.FullName = request.EmployeeName;

                if (!string.IsNullOrEmpty(request.Email))
                    employee.Email = request.Email;

                if (request.Salary.HasValue)
                    employee.Salary = request.Salary.Value;

                if (request.IsActive.HasValue)
                    employee.IsActive = request.IsActive.Value;

                await _employeeRepository.UpdateAsync(employee);

                await _auditLogService.LogUpdateAsync("Employee", employee.EmployeeId, request.ClientId, "Client");

                return ApiResponseDto<EmployeeResponseDto>.SuccessResponse(
                    await MapToResponseDto(employee),
                    "Employee updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<EmployeeResponseDto>.ErrorResponse($"Error updating employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> DeactivateEmployeeAsync(int employeeId, string reason)
        {
            try
            {
                var success = await _employeeRepository.DeactivateEmployeeAsync(employeeId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Employee", employeeId, 0, "System", $"Deactivated. Reason: {reason}");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Employee deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error deactivating employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> ActivateEmployeeAsync(int employeeId)
        {
            try
            {
                var success = await _employeeRepository.ActivateEmployeeAsync(employeeId);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Employee", employeeId, 0, "System", "Employee activated");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Employee activated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error activating employee: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<bool>> UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary, string reason)
        {
            try
            {
                var success = await _employeeRepository.UpdateEmployeeSalaryAsync(employeeId, newSalary);
                if (success)
                {
                    await _auditLogService.LogUpdateAsync("Employee", employeeId, 0, "System", $"Salary updated to {newSalary}. Reason: {reason}");
                }
                return ApiResponseDto<bool>.SuccessResponse(success, "Salary updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.ErrorResponse($"Error updating salary: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<decimal>> GetTotalMonthlySalaryBurdenAsync(int clientId)
        {
            try
            {
                var totalSalary = await _employeeRepository.GetTotalMonthlySalaryBurdenAsync(clientId);
                return ApiResponseDto<decimal>.SuccessResponse(totalSalary);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<decimal>.ErrorResponse($"Error calculating salary burden: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<int>> GetActiveEmployeesCountAsync(int clientId)
        {
            try
            {
                var count = await _employeeRepository.GetTotalActiveEmployeesCountAsync(clientId);
                return ApiResponseDto<int>.SuccessResponse(count);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<int>.ErrorResponse($"Error counting employees: {ex.Message}");
            }
        }

        private async Task<EmployeeResponseDto> MapToResponseDto(Employee employee)
        {
            var client = employee.Client ?? await _clientRepository.GetByIdAsync(employee.ClientId);

            return new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.FullName,
                Email = employee.Email,
                Phone = null,
                AccountNumber = null,
                BankName = null,
                IFSC = null,
                Salary = employee.Salary,
                Designation = null,
                Department = null,
                EmployeeCode = employee.EmployeeId.ToString(),
                JoiningDate = DateTime.UtcNow,
                IsActive = employee.IsActive,
                ClientId = employee.ClientId,
                ClientName = client?.ClientName ?? "Unknown"
            };
        }
    }
}