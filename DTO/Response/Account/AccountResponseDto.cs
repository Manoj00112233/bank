namespace Banking_CapStone.DTO.Response.Account
{
    public class AccountResponseDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
