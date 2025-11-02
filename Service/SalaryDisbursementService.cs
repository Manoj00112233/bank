using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Salary_Disbursement;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.SalaryDisbursement;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;
using Banking_CapStone.Service;

public class SalaryDisbursementService : ISalaryDisbursementService
{
    private readonly ISalaryDisbursementRepository _disbursementRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITransactionService _transactionService;
    private readonly IAuditLogService _auditLogService;

    public SalaryDisbursementService(
        ISalaryDisbursementRepository disbursementRepository,
        IClientRepository clientRepository,
        IEmployeeRepository employeeRepository,
        ITransactionService transactionService,
        IAuditLogService auditLogService)
    {
        _disbursementRepository = disbursementRepository;
        _clientRepository = clientRepository;
        _employeeRepository = employeeRepository;
        _transactionService = transactionService;
        _auditLogService = auditLogService;
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> CreateSalaryDisbursementAsync(CreateSalaryDisbursementRequestDto request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Client not found");

            if (await _disbursementRepository.IsDisbursementExistsAsync(request.ClientId, request.SalaryMonth, request.SalaryYear))
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement already exists for this period");

            var employees = request.AllEmployees
                ? await _employeeRepository.GetActiveEmployeesAsync(request.ClientId)
                : await GetSelectedEmployees(request.SelectedEmployeeIds);

            if (!employees.Any())
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("No employees found");

            var totalAmount = employees.Sum(e => e.Salary);

            var disbursement = new SalaryDisbursement
            {
                ClientId = request.ClientId,
                TotalAmount = totalAmount + (request.BonusAmount ?? 0),
                DisbursementDate = request.ScheduledDisbursementDate ?? DateTime.UtcNow,
                DisbursementStatusId = 3,
                AllEmployees = request.AllEmployees,
                SelectedEmployeeIds = request.SelectedEmployeeIds,
                Remarks = request.Remarks
            };

            await _disbursementRepository.AddAsync(disbursement);

            foreach (var employee in employees)
            {
                var detail = new SalaryDisbursementDetails
                {
                    SalaryDisbursementId = disbursement.SalaryDisbursementId,
                    EmployeeId = employee.EmployeeId,
                    Amount = employee.Salary + (request.BonusAmount ?? 0),
                    SalaryMonth = request.SalaryMonth,
                    SalaryYear = request.SalaryYear,
                    Success = null
                };
                await _disbursementRepository.SaveChangesAsync();
            }

            await _auditLogService.LogCreateAsync("SalaryDisbursement", disbursement.SalaryDisbursementId, request.ClientId, "Client");

            return ApiResponseDto<SalaryDisbursementResponseDto>.SuccessResponse(
                await MapToResponseDto(disbursement),
                "Salary disbursement created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse($"Error creating disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementByIdAsync(int disbursementId)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetDisbursementWithDetailsAsync(disbursementId);
            if (disbursement == null)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement not found");

            return ApiResponseDto<SalaryDisbursementResponseDto>.SuccessResponse(await MapToResponseDto(disbursement));
        }
        catch (Exception ex)
        {
            return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse($"Error retrieving disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementWithDetailsAsync(int disbursementId)
    {
        return await GetDisbursementByIdAsync(disbursementId);
    }

    public async Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetDisbursementsByClientIdAsync(int clientId)
    {
        try
        {
            var disbursements = await _disbursementRepository.GetDisbursementsByClientIdAsync(clientId);
            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.SuccessResponse(disbursementDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetPendingDisbursementsAsync()
    {
        try
        {
            var disbursements = await _disbursementRepository.GetPendingDisbursementsAsync();
            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.SuccessResponse(disbursementDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving pending disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetPendingDisbursementsByBankIdAsync(int bankId)
    {
        try
        {
            var disbursements = await _disbursementRepository.GetPendingDisbursementsByBankIdAsync(bankId);
            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.SuccessResponse(disbursementDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving pending disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetApprovedDisbursementsAsync(int clientId)
    {
        try
        {
            var disbursements = await _disbursementRepository.GetApprovedDisbursementsAsync(clientId);
            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.SuccessResponse(disbursementDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving approved disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>> GetRejectedDisbursementsAsync(int clientId)
    {
        try
        {
            var disbursements = await _disbursementRepository.GetRejectedDisbursementsAsync(clientId);
            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.SuccessResponse(disbursementDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving rejected disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<PaginatedResponseDto<SalaryDisbursementListResponseDto>>> GetDisbursementsPaginatedAsync(
        int? clientId,
        int? bankId,
        PaginationRequestDto pagination,
        FilterRequestDto? filter = null)
    {
        try
        {
            var (disbursements, totalCount) = await _disbursementRepository.GetDisbursementsPaginatedAsync(
                clientId, bankId, pagination, filter);

            var disbursementDtos = disbursements.Select(MapToListDto).ToList();

            var paginatedResponse = new PaginatedResponseDto<SalaryDisbursementListResponseDto>(
                disbursementDtos, totalCount, pagination.PageNumber, pagination.PageSize);

            return ApiResponseDto<PaginatedResponseDto<SalaryDisbursementListResponseDto>>.SuccessResponse(paginatedResponse);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<PaginatedResponseDto<SalaryDisbursementListResponseDto>>.ErrorResponse($"Error retrieving disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> ApproveDisbursementAsync(ApproveSalaryDisbursementRequestDto request)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetByIdAsync(request.SalaryDisbursementId);
            if (disbursement == null)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement not found");

            if (disbursement.DisbursementStatusId != 3)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement is not pending");

            var success = await _disbursementRepository.ApproveDisbursementAsync(request.SalaryDisbursementId, request.BankUserId);
            if (!success)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Failed to approve disbursement");

            if (request.ProcessInBatch)
                await ProcessBatchDisbursementAsync(request.SalaryDisbursementId);

            await _auditLogService.LogSalaryDisbursementAsync(request.SalaryDisbursementId, request.BankUserId, "BankUser", true);

            disbursement = await _disbursementRepository.GetByIdAsync(request.SalaryDisbursementId);
            return ApiResponseDto<SalaryDisbursementResponseDto>.SuccessResponse(
                await MapToResponseDto(disbursement),
                "Disbursement approved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse($"Error approving disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> RejectDisbursementAsync(RejectSalaryDisbursementRequestDto request)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetByIdAsync(request.SalaryDisbursementId);
            if (disbursement == null)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement not found");

            if (disbursement.DisbursementStatusId != 3)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement is not pending");

            var success = await _disbursementRepository.RejectDisbursementAsync(
                request.SalaryDisbursementId, request.BankUserId, request.RejectionReason);

            if (!success)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Failed to reject disbursement");

            await _auditLogService.LogSalaryDisbursementAsync(request.SalaryDisbursementId, request.BankUserId, "BankUser", false);

            disbursement = await _disbursementRepository.GetByIdAsync(request.SalaryDisbursementId);
            return ApiResponseDto<SalaryDisbursementResponseDto>.SuccessResponse(
                await MapToResponseDto(disbursement),
                "Disbursement rejected successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse($"Error rejecting disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> ProcessDisbursementAsync(int disbursementId)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetDisbursementWithDetailsAsync(disbursementId);
            if (disbursement == null)
                return ApiResponseDto<bool>.ErrorResponse("Disbursement not found");

            return ApiResponseDto<bool>.SuccessResponse(true, "Disbursement processed");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Error processing disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> ProcessBatchDisbursementAsync(int disbursementId)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetDisbursementWithDetailsAsync(disbursementId);
            if (disbursement == null)
                return ApiResponseDto<bool>.ErrorResponse("Disbursement not found");

            return ApiResponseDto<bool>.SuccessResponse(true, "Batch disbursement processed");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Error processing batch disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<int>> GetPendingDisbursementsCountAsync(int? clientId = null, int? bankId = null)
    {
        try
        {
            var count = await _disbursementRepository.GetPendingDisbursementsCountAsync(clientId, bankId);
            return ApiResponseDto<int>.SuccessResponse(count);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<int>.ErrorResponse($"Error counting disbursements: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<decimal>> GetTotalPendingDisbursementAmountAsync(int clientId)
    {
        try
        {
            var amount = await _disbursementRepository.GetTotalPendingDisbursementAmountAsync(clientId);
            return ApiResponseDto<decimal>.SuccessResponse(amount);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<decimal>.ErrorResponse($"Error calculating pending amount: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<Dictionary<string, object>>> GetDisbursementStatisticsAsync(int clientId)
    {
        try
        {
            var stats = await _disbursementRepository.GetDisbursementStatisticsAsync(clientId);
            return ApiResponseDto<Dictionary<string, object>>.SuccessResponse(stats);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<Dictionary<string, object>>.ErrorResponse($"Error retrieving statistics: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<SalaryDisbursementResponseDto>> GetDisbursementByMonthYearAsync(int clientId, int month, int year)
    {
        try
        {
            var disbursement = await _disbursementRepository.GetDisbursementByMonthYearAsync(clientId, month, year);
            if (disbursement == null)
                return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse("Disbursement not found");

            return ApiResponseDto<SalaryDisbursementResponseDto>.SuccessResponse(await MapToResponseDto(disbursement));
        }
        catch (Exception ex)
        {
            return ApiResponseDto<SalaryDisbursementResponseDto>.ErrorResponse($"Error retrieving disbursement: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> ValidateDisbursementRequestAsync(CreateSalaryDisbursementRequestDto request)
    {
        try
        {
            if (await _disbursementRepository.IsDisbursementExistsAsync(request.ClientId, request.SalaryMonth, request.SalaryYear))
                return ApiResponseDto<bool>.ErrorResponse("Disbursement already exists for this period");

            return ApiResponseDto<bool>.SuccessResponse(true);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Validation error: {ex.Message}");
        }
    }

    private async Task<IEnumerable<Employee>> GetSelectedEmployees(string? employeeIds)
    {
        if (string.IsNullOrEmpty(employeeIds))
            return new List<Employee>();

        var ids = employeeIds.Split(',').Select(int.Parse).ToList();
        var employees = new List<Employee>();

        foreach (var id in ids)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee != null)
                employees.Add(employee);
        }

        return employees;
    }

    private async Task<SalaryDisbursementResponseDto> MapToResponseDto(SalaryDisbursement disbursement)
    {
        var details = await _disbursementRepository.GetDisbursementDetailsAsync(disbursement.SalaryDisbursementId);

        return new SalaryDisbursementResponseDto
        {
            SalaryDisbursementId = disbursement.SalaryDisbursementId,
            ClientId = disbursement.ClientId,
            ClientName = disbursement.Client?.ClientName ?? "Unknown",
            TotalAmount = disbursement.TotalAmount,
            TotalEMployees = details.Count(),
            SuccessfulDisbursements = details.Count(d => d.Success == true),
            FailedDisbursements = details.Count(d => d.Success == false),
            DisbursementStatus = disbursement.DisbursementStatus?.Status.ToString() ?? "Unknown",
            AllEmployees = disbursement.AllEmployees,
            SalaryMonth = details.FirstOrDefault()?.SalaryMonth ?? 0,
            SalaryYear = details.FirstOrDefault()?.SalaryYear ?? 0,
            DisbursementDate = disbursement.DisbursementDate,
            ApprovedAt = disbursement.ApprovedAt,
            ApprovedByBankUserName = disbursement.ApprovedBy?.FullName,
            Remarks = disbursement.Remarks,
            Details = details.Select(d => new SalaryDisbursementDetailDto
            {
                DetailId = d.DetailId,
                EmployeeId = d.EmployeeId,
                EmployeeName = d.Employee?.FullName ?? "Unknown",
                Amount = d.Amount,
                Success = d.Success,
                FailureReason = d.FailureReason,
                TransactionId = d.TransactionId,
                ProcessedAt = d.ProcessedAt
            }).ToList()
        };
    }

    private SalaryDisbursementListResponseDto MapToListDto(SalaryDisbursement disbursement)
    {
        return new SalaryDisbursementListResponseDto
        {
            SalaryDisbursementId = disbursement.SalaryDisbursementId,
            ClientName = disbursement.Client?.ClientName ?? "Unknown",
            TotalAmount = disbursement.TotalAmount,
            TotalEmployees = disbursement.DisbursementDetials?.Count ?? 0,
            DisbursementStatus = disbursement.DisbursementStatus?.Status.ToString() ?? "Unknown",
            SalaryPeriod = $"{disbursement.DisbursementDetials?.FirstOrDefault()?.SalaryMonth}/{disbursement.DisbursementDetials?.FirstOrDefault()?.SalaryYear}",
            CreatedAt = disbursement.DisbursementDate
        };
    }
}
