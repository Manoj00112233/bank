using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Document
{
    public class UploadDocumentRequestDto
    {
        [Required(ErrorMessage ="Client ID is required")]
        [Range(1,int.MaxValue,ErrorMessage ="Invalid Client ID")]
        public int ClientId { get; set; }

        [Required(ErrorMessage ="Proof type is required")]
        [Range(1,9,ErrorMessage ="Proof type must be between 1 and 9")]
        public int ProofTypeId { get; set; }

        [Required(ErrorMessage ="File is required")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Document title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Document title must be between 3 and 100 characters")]
        public string DocumentTitle { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // e.g., PAN number, Aadhaar number

        [StringLength(50)]
        public string? DocumentNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? IssueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(100)]
        public string? IssuingAuthority { get; set; }

        public bool IsVerified { get; set; } = false;

        public string? Remarks { get; set; }

        public int? UploadedByBankeUserId { get; set; }


    }
}
