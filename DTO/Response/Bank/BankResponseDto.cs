namespace Banking_CapStone.DTO.Response.Bank
{
    public class BankResponseDto
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string SupportEmail { get; set; }

        public string? Website {  get; set; }

        public string? CustomerCareNumber { get; set; }

        public bool IsActive { get; set; }

        public int SuperAdminId { get; set; }
        public string SuperAdminName { get; set; }
        public int TotalBankUsers { get; set; }
        public int TotalClients { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
