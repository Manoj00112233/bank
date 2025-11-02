using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Banking_CapStone.Model
{
    public class Client : UserBase
    {
        internal string GstNumber;
        internal string PanNumber;
        internal string Address;

        public int ClientId { get; set; }

        [Required, StringLength(100)]
        public string ClientName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        public decimal AccountBalance { get; set; } = 0;

        [ForeignKey("Bank")]
        public int BankId { get; set; }
        public Bank? Bank { get; set; }

        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<Document>? Documents { get; set; }
        public virtual ICollection<Account>? Accounts { get; set; }
        public virtual ICollection<Beneficiary>? Beneficiaries { get; set; }
        public virtual ICollection<SalaryDisbursement>? SalaryDisbursements { get; set; }

    }
}
