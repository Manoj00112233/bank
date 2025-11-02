using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Response.Query
{
    public class QueryResponseDto
    {
        public int QueryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone {  get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? Response { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }

        public string? RespondedByUserName { get; set; }
    }
}
