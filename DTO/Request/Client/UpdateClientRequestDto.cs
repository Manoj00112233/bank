using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Client
{
    public class UpdateClientRequestDto
    {
        [Required(ErrorMessage ="Client ID is required")]
        [Range(1,int.MaxValue,ErrorMessage ="Invalid Client Id")]
        public int ClientId { get; set; }

        [StringLength(100,MinimumLength =5 , ErrorMessage ="Client name is required")]
        public string? ClientName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }


    }
}
