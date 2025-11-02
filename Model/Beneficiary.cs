using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    //vendor
    public class Beneficiary
    {
        [Key]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Client ID is Required!")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }

        [Required(ErrorMessage = "Beneficiary Name is Required!")]
        [StringLength(100)]
        public string BeneficiaryName { get; set; }

        [Required(ErrorMessage = "Account Number is Required!")]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Bank Name is Required!")]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required(ErrorMessage = "IFSC Code is Required!")]
        [StringLength(11)]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format")]
        public string IFSC { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(10)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
