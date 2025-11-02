namespace Banking_CapStone.DTO.Response.Beneficiary
{
    public class BeneficiaryResponseDto
    {
        public int BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSC {  get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int TotalPaymentsMade { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public DateTime  CreatedAt { get; set; }
        public DateTime? UpdatedAt {  get; set; }

    }
}
