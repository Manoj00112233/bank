using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Document;
using Banking_CapStone.DTO.Response.Common;
using Banking_CapStone.DTO.Response.Document;
using Banking_CapStone.Model;
using Banking_CapStone.Repository;
using Banking_CapStone.Service;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IAuditLogService _auditLogService;

    public DocumentService(
        IDocumentRepository documentRepository,
        IClientRepository clientRepository,
        ICloudinaryService cloudinaryService,
        IAuditLogService auditLogService)
    {
        _documentRepository = documentRepository;
        _clientRepository = clientRepository;
        _cloudinaryService = cloudinaryService;
        _auditLogService = auditLogService;
    }

    public async Task<ApiResponseDto<DocumentResponseDto>> UploadDocumentAsync(UploadDocumentRequestDto request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null)
                return ApiResponseDto<DocumentResponseDto>.ErrorResponse("Client not found");

            if (!await _cloudinaryService.ValidateFileAsync(request.File))
                return ApiResponseDto<DocumentResponseDto>.ErrorResponse("Invalid file");

            var uploadResult = await _cloudinaryService.UploadDocumentAsync(request.File, "client-documents");

            var document = new Document
            {
                FileName = request.File.FileName,
                FileUrl = uploadResult,
                ProofTypeId = request.ProofTypeId,
                ClientId = request.ClientId,
                UploadedAt = DateTime.UtcNow
            };

            await _documentRepository.AddAsync(document);

            await _auditLogService.LogCreateAsync("Document", document.DocumentId, request.ClientId, "Client");

            return ApiResponseDto<DocumentResponseDto>.SuccessResponse(
                await MapToResponseDto(document),
                "Document uploaded successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<DocumentResponseDto>.ErrorResponse($"Error uploading document: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<DocumentResponseDto>> GetDocumentByIdAsync(int documentId)
    {
        try
        {
            var document = await _documentRepository.GetDocumentWithDetailsAsync(documentId);
            if (document == null)
                return ApiResponseDto<DocumentResponseDto>.ErrorResponse("Document not found");

            return ApiResponseDto<DocumentResponseDto>.SuccessResponse(await MapToResponseDto(document));
        }
        catch (Exception ex)
        {
            return ApiResponseDto<DocumentResponseDto>.ErrorResponse($"Error retrieving document: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<DocumentResponseDto>> GetDocumentWithDetailsAsync(int documentId)
    {
        return await GetDocumentByIdAsync(documentId);
    }

    public async Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetDocumentsByClientIdAsync(int clientId)
    {
        try
        {
            var documents = await _documentRepository.GetDocumentsByClientIdAsync(clientId);
            var documentDtos = documents.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.SuccessResponse(documentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.ErrorResponse($"Error retrieving documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetVerifiedDocumentsAsync(int clientId)
    {
        try
        {
            var documents = await _documentRepository.GetVerifiedDocumentsAsync(clientId);
            var documentDtos = documents.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.SuccessResponse(documentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.ErrorResponse($"Error retrieving documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetUnverifiedDocumentsAsync(int clientId)
    {
        try
        {
            var documents = await _documentRepository.GetUnverifiedDocumentsAsync(clientId);
            var documentDtos = documents.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.SuccessResponse(documentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.ErrorResponse($"Error retrieving documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<DocumentListResponseDto>>> GetDocumentsByProofTypeAsync(int proofTypeId)
    {
        try
        {
            var documents = await _documentRepository.GetDocumentsByProofTypeAsync(proofTypeId);
            var documentDtos = documents.Select(MapToListDto).ToList();

            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.SuccessResponse(documentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<DocumentListResponseDto>>.ErrorResponse($"Error retrieving documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<PaginatedResponseDto<DocumentListResponseDto>>> GetDocumentsPaginatedAsync(
        int clientId,
        PaginationRequestDto pagination,
        FilterRequestDto? filter = null)
    {
        try
        {
            var (documents, totalCount) = await _documentRepository.GetDocumentsPaginatedAsync(clientId, pagination, filter);

            var documentDtos = documents.Select(MapToListDto).ToList();

            var paginatedResponse = new PaginatedResponseDto<DocumentListResponseDto>(
                documentDtos, totalCount, pagination.PageNumber, pagination.PageSize);

            return ApiResponseDto<PaginatedResponseDto<DocumentListResponseDto>>.SuccessResponse(paginatedResponse);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<PaginatedResponseDto<DocumentListResponseDto>>.ErrorResponse($"Error retrieving documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> VerifyDocumentAsync(int documentId, int verifiedByBankUserId, string remarks)
    {
        try
        {
            var success = await _documentRepository.VerifyDocumentAsync(documentId, verifiedByBankUserId);
            if (success)
            {
                await _auditLogService.LogUpdateAsync("Document", documentId, verifiedByBankUserId, "BankUser", $"Document verified. Remarks: {remarks}");
            }
            return ApiResponseDto<bool>.SuccessResponse(success, "Document verified successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Error verifying document: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> RejectDocumentAsync(int documentId, int rejectedByBankUserId, string reason)
    {
        try
        {
            var success = await _documentRepository.RejectDocumentAsync(documentId, rejectedByBankUserId, reason);
            if (success)
            {
                await _auditLogService.LogUpdateAsync("Document", documentId, rejectedByBankUserId, "BankUser", $"Document rejected. Reason: {reason}");
            }
            return ApiResponseDto<bool>.SuccessResponse(success, "Document rejected successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Error rejecting document: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteDocumentAsync(int documentId)
    {
        try
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                return ApiResponseDto<bool>.ErrorResponse("Document not found");

            var success = await _documentRepository.DeleteAsync(documentId);
            if (success)
            {
                await _auditLogService.LogDeleteAsync("Document", documentId, 0, "System");
            }
            return ApiResponseDto<bool>.SuccessResponse(success, "Document deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Error deleting document: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<int>> GetTotalDocumentsCountAsync(int clientId)
    {
        try
        {
            var count = await _documentRepository.GetTotalDocumentsCountAsync(clientId);
            return ApiResponseDto<int>.SuccessResponse(count);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<int>.ErrorResponse($"Error counting documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<int>> GetVerifiedDocumentsCountAsync(int clientId)
    {
        try
        {
            var count = await _documentRepository.GetVerifiedDocumentsCountAsync(clientId);
            return ApiResponseDto<int>.SuccessResponse(count);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<int>.ErrorResponse($"Error counting verified documents: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> ValidateDocumentAsync(int clientId, int proofTypeId)
    {
        try
        {
            var isValid = !await _documentRepository.IsDocumentExistsAsync(clientId, proofTypeId, "");
            return ApiResponseDto<bool>.SuccessResponse(isValid);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResponse($"Validation error: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<string>> GetDocumentUrlAsync(int documentId)
    {
        try
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                return ApiResponseDto<string>.ErrorResponse("Document not found");

            return ApiResponseDto<string>.SuccessResponse(document.FileUrl);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<string>.ErrorResponse($"Error retrieving document URL: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<IEnumerable<string>>> GetAllProofTypesAsync()
    {
        try
        {
            var proofTypes = await _documentRepository.GetAllProofTypesAsync();
            var proofTypeNames = proofTypes.Select(pt => pt.Type.ToString()).ToList();

            return ApiResponseDto<IEnumerable<string>>.SuccessResponse(proofTypeNames);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<IEnumerable<string>>.ErrorResponse($"Error retrieving proof types: {ex.Message}");
        }
    }

    private async Task<DocumentResponseDto> MapToResponseDto(Document document)
    {
        var client = document.Client ?? await _clientRepository.GetByIdAsync(document.ClientId);

        return new DocumentResponseDto
        {
            DocumentId = document.DocumentId,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            ProofType = document.ProofType?.Type.ToString() ?? "Unknown",
            ClientId = document.ClientId,
            ClientName = client?.ClientName ?? "Unknown",
            UploadedAt = document.UploadedAt,
            IsVerified = false,
            DocumentNumber = null,
            ExpiryDate = null
        };
    }

    private DocumentListResponseDto MapToListDto(Document document)
    {
        return new DocumentListResponseDto
        {
            DocumentId = document.DocumentId,
            FileName = document.FileName,
            ProofType = document.ProofType?.Type.ToString() ?? "Unknown",
           IsVerified = false,
           UploadedAt = document.UploadedAt
        };
    }
}