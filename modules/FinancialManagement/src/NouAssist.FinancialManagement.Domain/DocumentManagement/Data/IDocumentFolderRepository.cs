using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.DocumentManagement.Data
{
    public interface IDocumentFolderRepository : IRepository<DocumentFolder, Guid>
    {
        Task<List<DocumentFolder>> GetChildFoldersAsync(Guid parentFolderId);
        
        Task<List<DocumentFolder>> GetRootFoldersAsync(Guid? tenantId);
        
        Task<DocumentFolder> FindByPathAsync(string path);
        
        Task<List<DocumentFolder>> SearchAsync(string searchTerm);
    }
}
