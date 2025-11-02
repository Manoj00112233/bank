namespace Banking_CapStone.DTO.Response.Employee
{
    public class EmployeeListResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Designation { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
