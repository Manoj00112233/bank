using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    public enum Role
    {
        SUPER_ADMIN,
        BANK_USER,
        CLIENT_USER
    }
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role is Required!")]
        public Role Role { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
        public virtual ICollection<UserBase>? Users { get; set; }
    }
}
