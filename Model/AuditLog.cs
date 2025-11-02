using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        [Required]
        public string ActionType { get; set; } // CREATE, UPDATE, DELETE, LOGIN, etc.

        [Required]
        public string PerformedBy { get; set; } // Username or Email

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? Details { get; set; }

        [ForeignKey("UserBase")]
        public int UserId { get; set; }
        public UserBase? User { get; set; }

    }
}
