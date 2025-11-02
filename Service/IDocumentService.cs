using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Document;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Document;

namespace Banking_CapStone.Service
{
    public interface IDocumentService
    {
        Task<ApiResponseDto<DocumentResponseDto>> UploadDocumentAsync(UploadDocumentRequestDto request);
        Task<ApiResponseDto<DocumentResponseDto>> GetDocumentByIdAsync(int documentId);
        Task<ApiResponseDto<DocumentResponseDto>> GetDocumentWithDetailsAsync(int documentId);
        Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetDocumentsByClientIdAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetVerifiedDocumentsAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetUnverifiedDocumentsAsync(int clientId);
        Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetDocumentsByProofTypeAsync(int proofTypeId);
        Task<ApiResponseDto<PaginatedResponseDto<DocumentListResponseDto>>> GetDocumentsPaginatedAsync(
            int clientId,
            PaginationRequestDto pagination,
            FilterRequestDto? filter = null);
        Task<ApiResponseDto<bool>> VerifyDocumentAsync(int documentId, int verifiedByBankUserId, string remarks);
        Task<ApiResponseDto<bool>> RejectDocumentAsync(int documentId, int rejectedByBankUserId, string reason);
        Task<ApiResponseDto<bool>> DeleteDocumentAsync(int documentId);
        Task<ApiResponseDto<int>> GetTotalDocumentsCountAsync(int clientId);
        Task<ApiResponseDto<int>> GetVerifiedDocumentsCountAsync(int clientId);
        Task<ApiResponseDto<bool>> ValidateDocumentAsync(int clientId, int proofTypeId);
        Task<ApiResponseDto<string>> GetDocumentUrlAsync(int documentId);
        Task<ApiResponseDto<IEnumerable<string>>> GetAllProofTypesAsync();
    }
}
