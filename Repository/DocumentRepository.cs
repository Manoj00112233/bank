using Banking_CapStone.Data;
using Banking_CapStone.DTO.Request.Common;
using Banking_CapStone.Model;
using Microsoft.EntityFrameworkCore;

namespace Banking_CapStone.Repository
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(BankingDbContext context) : base(context) { }

        public async Task<Document?> GetDocumentWithDetailsAsync(int documentId)
        {
            return await _context.Documents
                .Include(d => d.Client)
                .Include(d => d.ProofType)
                .FirstOrDefaultAsync(d => d.DocumentId == documentId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByClientIdAsync(int clientId)
        {
            return await _context.Documents
                .Where(d=> d.ClientId == clientId)
                .Include(d=>d.ProofType)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByProofTypeAsync(int proofTypeId)
        {
            return await _context.Documents
                .Where(d => d.ProofTypeId == proofTypeId)
                .Include(d => d.Client)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetVerifiedDocumentsAsync(int clientId)
        {
               return await _context.Documents
                .Where(d => d.ClientId == clientId)
                .Include(d => d.ProofType)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetUnverifiedDocumentsAsync(int clientId)
        {
            return await _context.Documents
                .Where (d => d.ClientId == clientId)
                .Include(d => d.ProofType)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Document> Documents, int TotalCount)> GetDocumentsPaginatedAsync(
           int clientId,
           PaginationRequestDto pagination,
           FilterRequestDto? filter = null)
        {
            var query = _context.Documents
                .Where(d => d.ClientId == clientId)
                .Include(d => d.Client)
                .Include(d => d.ProofType)
                .AsQueryable();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(d =>
                        d.FileName.Contains(filter.SearchTerm) ||
                        d.ProofType.Type.ToString().Contains(filter.SearchTerm));
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(d => d.UploadedAt >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(d => d.UploadedAt <= filter.ToDate.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var documents = await query
                .OrderByDescending(d => d.UploadedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (documents, totalCount);
        }

        public async Task<bool> VerifyDocumentAsync(int documentId, int verifiedByBankUserId)
        {
            var document = await GetByIdAsync(documentId);
            if (document == null) return false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectDocumentAsync(int documentId, int rejectedByBankUserId, string reason)
        {
            var document = await GetByIdAsync(documentId);
            if (document == null) return false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalDocumentsCountAsync(int clientId)
        {
            return await _context.Documents
                .CountAsync(d => d.ClientId == clientId);
        }

        public async Task<int> GetVerifiedDocumentsCountAsync(int clientId)
        {
            return await _context.Documents
                .CountAsync(d => d.ClientId == clientId);
        }

        public async Task<ProofType?> GetProofTypeByIdAsync(int proofTypeId)
        {
            return await _context.ProofTypes.FindAsync(proofTypeId);
        }

        public async Task<IEnumerable<ProofType>> GetAllProofTypesAsync()
        {
            return await _context.ProofTypes.ToListAsync();
        }

        public async Task<bool> IsDocumentExistsAsync(int clientId, int proofTypeId, string fileName)
        {
            return await _context.Documents
                .AnyAsync(d => d.ClientId == clientId &&
                              d.ProofTypeId == proofTypeId &&
                              d.FileName == fileName);
        }
    }
}
