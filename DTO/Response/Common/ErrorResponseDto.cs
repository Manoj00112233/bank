namespace Banking_CapStone.DTO.Response.Common
{
    public class ErrorResponseDto
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? TraceId { get; set; }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
