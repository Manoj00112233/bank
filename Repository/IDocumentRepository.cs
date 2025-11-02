using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;

namespace Banking_CapStone.Repository
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task<Document?> GetDocumentWithDetailsAsync(int documentId);
        Task<IEnumerable<Document>> GetDocumentsByClientIdAsync(int clientId);
        Task<IEnumerable<Document>> GetDocumentsByProofTypeAsync(int proofTypeId);
        Task<IEnumerable<Document>> GetVerifiedDocumentsAsync(int clientId);
        Task<IEnumerable<Document>> GetUnverifiedDocumentsAsync(int clientId);
        Task<(IEnumerable<Document> Documents, int TotalCount)> GetDocumentsPaginatedAsync(
                                     int clientId,
                                     PaginationRequestDto pagination,
                                       FilterRequestDto? filter = null);
        Task<bool> VerifyDocumentAsync(int documentId, int verifiedByBankUserId);
        Task<bool> RejectDocumentAsync(int documentId, int rejectedByBankUserId, string reason);
        Task<int> GetTotalDocumentsCountAsync(int clientId);
        Task<int> GetVerifiedDocumentsCountAsync(int clientId);
        Task<ProofType?> GetProofTypeByIdAsync(int proofTypeId);
        Task<IEnumerable<ProofType>> GetAllProofTypesAsync();
        Task<bool> IsDocumentExistsAsync(int clientId, int proofTypeId, string fileName);
    }

}
