using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Bank
{
    public class CreateBankRequestDto
    {
        [Required(ErrorMessage ="Bank name is required")]
        [StringLength(100,MinimumLength =6 , ErrorMessage = "Bank name must be between 3 and 100 characters")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "IFSC code is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "IFSC code must be exactly 11 characters")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format (e.g., HDFC0001234)")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Contact number must be exactly 10 digits")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage ="Support email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        [StringLength(100)]
        public string SupportEmail { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(200)]
        public string? Website {  get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10,12}$", ErrorMessage = "Customer care number must be 10-12 digits")]
        public string? CustomerCareNumber { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

    }
}
