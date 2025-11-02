namespace Banking_CapStone.DTO.Response.Beneficiary
{
    public class BeneficiaryListResponseDto
    {
        public int BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public bool IsActive { get; set; }

    }
}
