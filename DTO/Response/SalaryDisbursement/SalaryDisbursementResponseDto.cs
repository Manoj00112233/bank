namespace Banking_CapStone.DTO.Response.SalaryDisbursement
{
    public class SalaryDisbursementResponseDto
    {
        public int SalaryDisbursementId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalEMployees { get; set; }
        public int SuccessfulDisbursements { get; set; }
        public int FailedDisbursements { get; set; }
        public string DisbursementStatus { get; set; }
        public bool AllEmployees { get; set; }
        public int SalaryMonth {  get; set; }
        public int SalaryYear { get; set; }
        public DateTime DisbursementDate { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedByBankUserName { get; set; }
        public string? Remarks { get; set; }
        public List<SalaryDisbursementDetailDto> Details { get; set; }
    }
    public class SalaryDisbursementDetailDto
    {
        public int DetailId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public bool? Success { get; set; }
        public string? FailureReason { get; set; }
        public int? TransactionId { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
