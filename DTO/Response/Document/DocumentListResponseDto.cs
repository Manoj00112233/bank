namespace Banking_CapStone.DTO.Response.Document
{
    public class DocumentListResponseDto
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string ProofType { get; set; }
        public bool IsVerified { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
