using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [ForeignKey("BankUser")]
        public int? BankUserId { get; set; }
        public BankUser? BankUser { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [ForeignKey("Account")]
        public int? AccountId { get; set; }
        public virtual Account? Account { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public virtual TransactionType? TransactionType { get; set; }

        [ForeignKey("Payment")]
        public int? PaymentId { get; set; }
        public virtual Payment? Payment { get; set; }

        [ForeignKey("SalaryDisbursement")]
        public int? SalaryDisbursementId { get; set; }
        public virtual SalaryDisbursement? SalaryDisbursement { get; set; }

        [ForeignKey("SalaryDisbursementDetail")]
        public int? SalaryDisbursementDetailId { get; set; }
        public virtual SalaryDisbursementDetails? SalaryDisbursementDetail { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Approved, Declined

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
