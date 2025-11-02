namespace Banking_CapStone.DTO.Response.Employee
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public decimal Salary { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
