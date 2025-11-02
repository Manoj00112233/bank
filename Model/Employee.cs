using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_CapStone.Model
{
    public class Employee
    {
        
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Payment>? Payments { get; set; }

        public virtual ICollection<SalaryDisbursementDetails>? SalaryDisbursementDetails { get; set; }

    }
}
