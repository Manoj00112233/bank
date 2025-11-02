using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Query
    {
        [Key]
        public int QueryId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(10)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Subject is required!")]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required!")]
        [StringLength(1000)]
        public string Message { get; set; }

        [StringLength(2000)]
        public string? Response { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        [ForeignKey("RespondedByUser")]
        public int? RespondedBy { get; set; }
        public virtual UserBase? RespondedByUser { get; set; }

        public bool IsResolved { get; set; } = false;

        // Priority: Low, Medium, High
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        // Category: Technical, Account, Payment, General
        [StringLength(50)]
        public string? Category { get; set; }
    }
}