namespace Banking_CapStone.DTO.Response.Client
{
    public class ClientResponseDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
