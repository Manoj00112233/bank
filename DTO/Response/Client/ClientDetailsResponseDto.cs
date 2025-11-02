namespace Banking_CapStone.DTO.Response.Client
{
    public class ClientDetailsResponseDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PanNumber { get; set; }
        public string? GstNumber { get; set; }
        public DateTime DateOfIncorporation { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public decimal TotalBalance { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalBeneficiaries { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AccountSummaryDto> Accounts { get; set; }
    }

    public class AccountSummaryDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public decimal Balance { get; set; }

    }
}
