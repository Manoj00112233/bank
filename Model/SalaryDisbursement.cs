using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class SalaryDisbursement
    {
        [Key]
        public int SalaryDisbursementId { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }

        [Required(ErrorMessage = "Total Amount is Required!")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DisbursementDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("DisbursementStatus")]
        public int DisbursementStatusId { get; set; } = 3; 
        public virtual PaymentStatus? DisbursementStatus { get; set; }

        [Required(ErrorMessage = "AllEmployees flag is Required!")]
        public bool AllEmployees { get; set; } = true;

        [StringLength(500)]
        public string? SelectedEmployeeIds { get; set; }

        public int? ApprovedByBankUserId { get; set; }
        public virtual BankUser? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }


        [StringLength(500)]
        public string? Remarks { get; set; }

        public virtual ICollection<SalaryDisbursementDetails> DisbursementDetials { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }


    }
}
