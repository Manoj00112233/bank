using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Payment
    {
        internal readonly object Bank;

        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        [ForeignKey("Beneficiary")]
        public int? BeneficiaryId { get; set; }
        public virtual Beneficiary? Beneficiary { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey("PaymentStatus")]
        public int PaymentStatusId { get; set; } = 3; // Default: Pending
        public virtual PaymentStatus? PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("BankUser")]
        public int? BankUserId { get; set; }
        public BankUser? BankUser { get; set; }

        // ✅ Navigation property for transactions
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}