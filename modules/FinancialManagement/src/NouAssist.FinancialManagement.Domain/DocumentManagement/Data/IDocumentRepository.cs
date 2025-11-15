using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DocumentManagement.Data
{
    public interface IDocumentRepository : IRepository<Document, Guid>
    {
        Task<Document> FindByHashAsync(string fileHash);
        
        Task<List<Document>> GetByEntityReferenceAsync(string entityType, string entityId);
        
        Task<List<Document>> GetDocumentsPendingOCRAsync();
        
        Task<bool> HasReferencesAsync(Guid documentId);
        
        Task<List<Document>> GetByFolderAsync(Guid folderId);
        
        Task<List<Document>> SearchAsync(
            string searchTerm,
            DocumentType? documentType = null,
            DateTime? uploadedAfter = null,
            DateTime? uploadedBefore = null,
            int maxResults = 100);
    }
}
