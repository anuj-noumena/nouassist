using System;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DocumentManagement
{
    public interface IDocumentFolderAppService : IApplicationService
    {
        Task<DocumentFolderDto> GetAsync(Guid id);
        
        Task<ListResultDto<DocumentFolderDto>> GetRootFoldersAsync();
        
        Task<ListResultDto<DocumentFolderDto>> GetChildFoldersAsync(Guid parentId);
        
        Task<DocumentFolderDto> CreateAsync(CreateDocumentFolderDto input);
        
        Task<DocumentFolderDto> UpdateAsync(Guid id, CreateDocumentFolderDto input);
        
        Task MoveAsync(Guid id, Guid? newParentId);
        
        Task AddDocumentAsync(Guid folderId, Guid documentId);
        
        Task RemoveDocumentAsync(Guid folderId, Guid documentId);
        
        Task DeleteAsync(Guid id);
    }
}
