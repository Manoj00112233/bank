using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public abstract class UserBase
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [ForeignKey("UserRole")]
        public int RoleId { get; set; }
        public virtual UserRole UserRole { get; set; }

        public bool IsActive { get; set; } = true;


        public ICollection<AuditLog>? AuditLogs { get; set; }

    }

}
