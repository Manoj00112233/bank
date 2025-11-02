using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Banking_CapStone.Model
{
    public class BankUser : UserBase
    {
        
        public int BankUserId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [ EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [ForeignKey("Bank")]
        public int BankId { get; set; }

        public Bank? Bank { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Payment>? Payments { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<SalaryDisbursement>? ApprovedSalaryDisbursement { get; set; }
    }
}
