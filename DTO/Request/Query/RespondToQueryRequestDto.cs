using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Query
{
    public class RespondToQueryRequestDto
    {
        [Required(ErrorMessage = "Query ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Query ID")]
        public int QueryId { get; set; }

        [Required(ErrorMessage = "Response is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Response must be between 10 and 2000 characters")]
        public string Response { get; set; }

        [Required(ErrorMessage = "Responded by user ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid User ID")]
        public int RespondedBy { get; set; }

        [Required(ErrorMessage = "Resolution status is required")]
        public bool IsResolved { get; set; }

        public bool SendEmailNotification { get; set; } = true;

        public bool RequiresFollowUp { get; set; } = false;
    }
}
