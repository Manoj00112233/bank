using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }

        [Required, StringLength(100)]
        public string BankName { get; set; }

        [Required, StringLength(15)]
        public string IFSCCode { get; set; }

        [Required , StringLength(150)]
        public string Address { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        [EmailAddress]
        public string SupportEmail { get; set; }

        [ForeignKey("SuperAdmin")]
        public int SuperAdminId { get; set; }
        public SuperAdmin? SuperAdmin { get; set; }

        public ICollection<BankUser>? BankUsers { get; set; }
        public ICollection<Client>? Clients { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }

    }
}
