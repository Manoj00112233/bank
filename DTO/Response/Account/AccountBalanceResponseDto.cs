namespace Banking_CapStone.DTO.Response.Account
{
    public class AccountBalanceResponseDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal HoldAmount { get; set; }
        public string Currency { get; set; } = "INR";
        public DateTime AsOfDate { get; set; } = DateTime.UtcNow;
    }
}
