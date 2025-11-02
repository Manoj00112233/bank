namespace Banking_CapStone.DTO.Response.Bank
{
    public class BankListResponseDto
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public int TotalClients {  get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
