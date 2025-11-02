using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    public enum PayStatus
    {
        APPROVED ,
        DECLINED ,
        PENDING
    }
    public class PaymentStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Status is Required!")]
        public PayStatus Status { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<SalaryDisbursement> SalaryDisbursements { get; set; }
    }
}
