using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Banking_CapStone.DTO.Request.Common
{
    public class FilterRequestDto
    {
        [StringLength(100)]
        public string? SearchTerm { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public int? StatusId { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        public int? BankId { get; set; }

        public int? ClientId { get; set; }
    }
}
