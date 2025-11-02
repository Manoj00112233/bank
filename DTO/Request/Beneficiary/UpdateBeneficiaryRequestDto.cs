using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Beneficiary
{
    public class UpdateBeneficiaryRequestDto
    {
        [Required(ErrorMessage = "Beneficiary ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Beneficiary ID")]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Client ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Client ID")]
        public int ClientId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Beneficiary name must be between 3 and 100 characters")]
        public string? BeneficiaryName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }

        [StringLength(200, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 200 characters")]
        public string? Address { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 500 characters")]
        public string? ReasonForUpdate { get; set; }

        public string? Remarks { get; set; }


    }
}
