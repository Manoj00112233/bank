using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required(ErrorMessage ="Account Number is Required")]
        [StringLength(20)]
        [RegularExpression(@"^BPA\d{8}[A-Z0-9]{6}$" , ErrorMessage ="AccountNumber must be BPA + 8 Digit + 6 alphanumeric")]
        public string AccountNumber { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }

        [ForeignKey("Bank")]
        public int BankId { get; set; }
        public virtual Bank? Bank { get; set; }

        [Required(ErrorMessage = "Balance is Required!")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999.99, ErrorMessage = "Balance must be positive")]
        public decimal Balance { get; set; } = 0;

        [Required(ErrorMessage = "Account Type is Required!")]
        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }
        public virtual AccountType? AccountType { get; set; }

        [Required(ErrorMessage = "Account Status is Required!")]
        [ForeignKey("AccountStatus")]
        public int AccountStatusId { get; set; }
        public virtual AccountStatus? AccountStatus { get; set; }

        [Required(ErrorMessage = "Account Creation Date is Required!")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
