using System.ComponentModel.DataAnnotations;

namespace Banking_CapStone.Model
{
    //id card for opening account

    public enum DocProofType
    {
        IDENTITY_PROOF,
        ADDRESS_PROOF,
        DATE_OF_BIRTH_PROOF,
        PHOTOGRAPH,
        PAN_CARD,
        SIGNATURE,
        BANK_STATEMENT,
        BUSINESS_PROOF,
        OTHER
    }
    public class ProofType
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage ="Proof Type is Required")]
        public DocProofType Type { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
