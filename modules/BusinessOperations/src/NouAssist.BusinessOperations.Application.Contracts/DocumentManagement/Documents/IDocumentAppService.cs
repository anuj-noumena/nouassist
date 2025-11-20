using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents
{
    /// <summary>
    /// Application service for document management operations
    /// </summary>
    public interface IDocumentAppService : IApplicationService
    {
        /// <summary>
        /// Gets a document by ID
        /// </summary>
        Task<DocumentDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of documents
        /// </summary>
        Task<PagedResultDto<DocumentDto>> GetListAsync(GetDocumentListDto input);
        
        /// <summary>
        /// Gets versions of a document
        /// </summary>
        Task<PagedResultDto<DocumentVersionDto>> GetVersionsAsync(GetDocumentVersionsDto input);
        
        /// <summary>
        /// Creates a new document by uploading a file
        /// </summary>
        Task<DocumentDto> CreateAsync(UploadDocumentDto input, FileInputContent fileContent);
        
        /// <summary>
        /// Creates a new version of a document
        /// </summary>
        Task<DocumentVersionDto> CreateVersionAsync(CreateDocumentVersionDto input, FileInputContent fileContent);
        
        /// <summary>
        /// Gets a download link for a document
        /// </summary>
        Task<DocumentDownloadLinkDto> GetDownloadLinkAsync(GetDocumentDownloadLinkDto input);
        
        /// <summary>
        /// Grants access to a document
        /// </summary>
        Task GrantAccessAsync(GrantDocumentAccessDto input);
        
        /// <summary>
        /// Revokes access to a document
        /// </summary>
        Task RevokeAccessAsync(RevokeDocumentAccessDto input);
        
        /// <summary>
        /// Archives a document
        /// </summary>
        Task ArchiveAsync(Guid id);
        
        /// <summary>
        /// Restores a document from archive
        /// </summary>
        Task RestoreAsync(Guid id);
        
        /// <summary>
        /// Marks a document for deletion
        /// </summary>
        Task MarkForDeletionAsync(Guid id);
        
        /// <summary>
        /// Permanently deletes a document
        /// </summary>
        Task DeleteAsync(Guid id);
    }
}
