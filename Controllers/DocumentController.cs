using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.DTO.Request.Document;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : BaseApiController
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "CLIENT_USER,BANK_USER")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _documentService.UploadDocumentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocument(int documentId)
        {
            var result = await _documentService.GetDocumentByIdAsync(documentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetDocumentsByClient(int clientId)
        {
            var result = await _documentService.GetDocumentsByClientIdAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("client/{clientId}/verified")]
        public async Task<IActionResult> GetVerifiedDocuments(int clientId)
        {
            var result = await _documentService.GetVerifiedDocumentsAsync(clientId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("client/{clientId}/list")]
        public async Task<IActionResult> GetDocumentsPaginated(
            int clientId,
            [FromBody] PaginationRequestDto pagination,
            [FromQuery] FilterRequestDto? filter = null)
        {
            var result = await _documentService.GetDocumentsPaginatedAsync(clientId, pagination, filter);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{documentId}/verify")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> VerifyDocument(int documentId, [FromQuery] string remarks)
        {
            var bankUserId = GetUserId();
            var result = await _documentService.VerifyDocumentAsync(documentId, bankUserId, remarks);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{documentId}/reject")]
        [Authorize(Roles = "BANK_USER")]
        public async Task<IActionResult> RejectDocument(int documentId, [FromQuery] string reason)
        {
            var bankUserId = GetUserId();
            var result = await _documentService.RejectDocumentAsync(documentId, bankUserId, reason);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{documentId}")]
        [Authorize(Roles = "BANK_USER,CLIENT_USER")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            var result = await _documentService.DeleteDocumentAsync(documentId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("proof-types")]
        public async Task<IActionResult> GetProofTypes()
        {
            var result = await _documentService.GetAllProofTypesAsync();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
