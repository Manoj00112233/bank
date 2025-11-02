using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Banking_CapStone.DTO.Response.Document
{
    public class DocumentResponseDto
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ProofType { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime UploadedAt { get; set; }

        public bool IsVerified { get; set; }

        public string? DocumentNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
