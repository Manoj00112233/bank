namespace Banking_CapStone.DTO.Response.SalaryDisbursement
{
    public class SalaryDisbursementListResponseDto
    {
        public int SalaryDisbursementId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalEmployees { get; set; }
        public string DisbursementStatus { get; set; }

        public string SalaryPeriod { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
