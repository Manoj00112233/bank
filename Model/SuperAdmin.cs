using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    public class SuperAdmin : UserBase
    {
        
        public int SuperAdminId { get; set; }

        [Required(ErrorMessage ="Full name is required")]
        [StringLength(100, ErrorMessage ="Name cannot exceed 100 character")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 50 character")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        [StringLength(100)]
        public string Email {  get; set; }

        [Required(ErrorMessage ="Password is required")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

       
        public ICollection<Bank>? Banks { get; set; }
    }
}
