using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    public enum TxnType
    {
        CREDIT,
        DEBIT
    }
    public class TransactionType
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Type is Required")]
        public TxnType Type { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }

    }
}
