using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; }

        [Required(ErrorMessage = "Document Type is Required!")]
        [ForeignKey("ProofType")]
        public int ProofTypeId { get; set; }
        public virtual ProofType? ProofType { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
