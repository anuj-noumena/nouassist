using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DocumentManagement.Data
{
    public interface IDocumentFolderRepository : IRepository<DocumentFolder, Guid>
    {
        Task<List<DocumentFolder>> GetChildFoldersAsync(Guid parentFolderId);
        
        Task<List<DocumentFolder>> GetRootFoldersAsync(Guid? tenantId);
        
        Task<DocumentFolder> FindByPathAsync(string path);
        
        Task<List<DocumentFolder>> SearchAsync(string searchTerm);
    }
}
