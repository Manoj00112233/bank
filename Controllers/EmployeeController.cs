using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Employee;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CLIENT_USER")]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.CreateEmployeeAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetEmployeesByClient(int clientId)
        {
            var result = await _employeeService.GetEmployeesByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/active")]
        public async Task<IActionResult> GetActiveEmployees(int clientId)
        {
            var result = await _employeeService.GetActiveEmployeesAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("client/{clientId}/list")]
        public async Task<IActionResult> GetEmployeesPaginated(
            int clientId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _employeeService.GetEmployeesPaginatedAsync(clientId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.UpdateEmployeeAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{employeeId}/deactivate")]
        public async Task<IActionResult> DeactivateEmployee(int employeeId, [FromQuery] string reason)
        {
            var result = await _employeeService.DeactivateEmployeeAsync(employeeId, reason);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{employeeId}/activate")]
        public async Task<IActionResult> ActivateEmployee(int employeeId)
        {
            var result = await _employeeService.ActivateEmployeeAsync(employeeId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/salary-burden")]
        public async Task<IActionResult> GetTotalSalaryBurden(int clientId)
        {
            var result = await _employeeService.GetTotalMonthlySalaryBurdenAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}