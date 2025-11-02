using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    public enum AccStatus
    {
        ACTIVE,
        INACTIVE,
        CLOSED
    }
    public class AccountStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required(ErrorMessage ="Status is Required")]
        public AccStatus Status { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
