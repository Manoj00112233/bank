using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.DTO.Request.Common
{
    public class PaginationRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        [StringLength(50)]
        public string? SortBy { get; set; }

        [StringLength(4)]
        [RegularExpression(@"^(asc|desc|ASC|DESC)$", ErrorMessage = "Sort order must be 'asc' or 'desc'")]
        public string SortOrder { get; set; } = "desc";
    }
}
