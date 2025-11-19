using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Data;
using NouAssist.FinancialManagement.DocumentManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.DocumentManagement.Repositories
{
    public class EfCoreDocumentRepository
        : EfCoreRepository<IFinancialManagementDbContext, Document, Guid>, IDocumentRepository
    {
        public EfCoreDocumentRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Document> FindByHashAsync(
            string fileHash,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileHash))
            {
                return null;
            }

            var query = await GetQueryableAsync();

            return await query.FirstOrDefaultAsync(
                d => d.Metadata.FileHash == fileHash,
                cancellationToken);
        }

        public async Task<List<Document>> GetByEntityReferenceAsync(
            string entityType,
            string entityId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(d => d.EntityReference.EntityType == entityType &&
                           d.EntityReference.EntityId == entityId &&
                           d.Status != DocumentStatus.Deleted)
                .OrderByDescending(d => d.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Document>> GetDocumentsPendingOCRAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(d => d.Status == DocumentStatus.Available &&
                           d.OCRResult == null &&
                           (d.Metadata.ContentType == "application/pdf" ||
                            d.Metadata.ContentType.StartsWith("image/")))
                .OrderBy(d => d.CreationTime)
                .Take(100) // Process in batches
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasReferencesAsync(
            Guid documentId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // Check if document has entity reference
            var document = await query.FirstOrDefaultAsync(
                d => d.Id == documentId,
                cancellationToken);

            return document?.EntityReference != null;
        }

        public async Task<List<Document>> GetByFolderAsync(
            Guid folderId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var folder = await dbContext.DocumentFolders.FindAsync(new object[] { folderId }, cancellationToken);

            if (folder == null || !folder.DocumentIds.Any())
            {
                return new List<Document>();
            }

            var query = await GetQueryableAsync();

            return await query
                .Where(d => folder.DocumentIds.Contains(d.Id))
                .OrderByDescending(d => d.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Document>> SearchAsync(
            string searchTerm,
            DocumentType? documentType = null,
            DateTime? uploadedAfter = null,
            DateTime? uploadedBefore = null,
            int maxResults = 100,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.Where(d => d.Status != DocumentStatus.Deleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(d =>
                    d.Metadata.FileName.ToLower().Contains(searchTerm) ||
                    d.Description.ToLower().Contains(searchTerm) ||
                    (d.OCRResult != null && d.OCRResult.ExtractedText.ToLower().Contains(searchTerm)));
            }

            if (documentType.HasValue)
            {
                query = query.Where(d => d.Type == documentType.Value);
            }

            if (uploadedAfter.HasValue)
            {
                query = query.Where(d => d.CreationTime >= uploadedAfter.Value);
            }

            if (uploadedBefore.HasValue)
            {
                query = query.Where(d => d.CreationTime <= uploadedBefore.Value);
            }

            return await query
                .OrderByDescending(d => d.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public Task<Document> FindByHashAsync(string fileHash)
        {
            return FindByHashAsync(fileHash, default);
        }

        public Task<List<Document>> GetByEntityReferenceAsync(string entityType, string entityId)
        {
            return GetByEntityReferenceAsync(entityType, entityId, default);
        }

        public Task<List<Document>> GetDocumentsPendingOCRAsync()
        {
            return GetDocumentsPendingOCRAsync(default);
        }

        public Task<bool> HasReferencesAsync(Guid documentId)
        {
            return HasReferencesAsync(documentId, default);
        }

        public Task<List<Document>> GetByFolderAsync(Guid folderId)
        {
            return GetByFolderAsync(folderId, default);
        }

        public Task<List<Document>> SearchAsync(string searchTerm, DocumentType? documentType = null, DateTime? uploadedAfter = null,
            DateTime? uploadedBefore = null, int maxResults = 100)
        {
            return SearchAsync(searchTerm, documentType, uploadedAfter, uploadedBefore, maxResults, default);
        }
    }
}
