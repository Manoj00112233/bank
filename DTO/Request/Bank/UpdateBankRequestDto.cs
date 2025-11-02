using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Bank
{
    public class UpdateBankRequestDto
    {
        [Required(ErrorMessage = "Bank ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bank ID")]
        public int BankId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bank name must be between 3 and 100 characters")]
        public string? BankName { get; set; }

        [StringLength(200, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 200 characters")]
        public string? Address { get; set; }

        [Phone(ErrorMessage ="invalid phone number format")]
        [RegularExpression("@^[0-9]{10}$",ErrorMessage ="Contact number must be of 10 digit")]
        public string? ContactNumber { get; set; }

        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        [StringLength(50)]
        public string? SupportEmail { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(200)]
        public string? Website { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10,12}$", ErrorMessage = "Customer care number must be 10-12 digits")]
        public string? CustomerCareNumber { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }



    }
}
