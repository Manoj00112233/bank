using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class SalaryDisbursementDetails
    {
        [Key]
        public int DetailId { get; set; }

        [ForeignKey("SalaryDisbursement")]
        public int SalaryDisbursementId { get; set; }
        public virtual SalaryDisbursement? SalaryDisbursement { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public bool? Success { get; set; } = null;

        [Required(ErrorMessage = "Amount is Required!")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey("Transaction")]
        public int? TransactionId { get; set; }
        public virtual Transaction? Transaction { get; set; }

        [StringLength(500)]
        public string? FailureReason { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public int SalaryMonth { get; set; } 
        public int SalaryYear { get; set; }

        [StringLength(200)]
        public string? Remarks { get; set; }
    }
}