namespace Banking_CapStone.DTO.Response.Auth
{
    public class UserProfileResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public int? BankId { get; set; }
        public string? BankName { get; set; }
        public int? ClientId { get; set; }
        public string? ClientName { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Phone {  get; set; }

    }
}
