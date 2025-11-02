using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{

    public enum AccType
    {
        SAVING,
        CURRENT,
        SALARY
    }
    public class AccountType
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage ="Type is required")]
        public AccType Type { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
