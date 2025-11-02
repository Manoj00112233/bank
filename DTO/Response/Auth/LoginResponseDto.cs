namespace Banking_CapStone.DTO.Response.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string TokenType { get; set; } = "Bearer";

        public DateTime ExpiresAt { get; set; }

        public UserProfileResponseDto User {  get; set; }

        public string Message { get; set; } = "Login Successful";
    }
}
