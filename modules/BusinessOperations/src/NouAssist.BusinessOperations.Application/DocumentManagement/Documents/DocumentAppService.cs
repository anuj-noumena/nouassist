using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.Exceptions;
using NouAssist.BusinessOperations.DocumentManagement.Interfaces.Services;
using NouAssist.BusinessOperations.DocumentManagement.Permissions;
using NouAssist.BusinessOperations.DocumentManagement.Repositories;
using NouAssist.BusinessOperations.DocumentManagement.Specifications;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents
{
    [Authorize(DocumentManagementPermissions.Documents.Default)]
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentManager _documentManager;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;

        public DocumentAppService(
            IDocumentRepository documentRepository,
            IDocumentManager documentManager,
            ICurrentUser currentUser,
            IClock clock)
        {
            _documentRepository = documentRepository;
            _documentManager = documentManager;
            _currentUser = currentUser;
            _clock = clock;
            
            LocalizationResource = typeof(DocumentManagementResource);
        }

        [Authorize(DocumentManagementPermissions.Documents.View)]
        public virtual async Task<DocumentDto> GetAsync(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Get document with versions
                var document = await _documentRepository.GetDocumentWithVersionsAsync(id);
                
                // Check access
                var hasAccess = await _documentManager.CheckDocumentAccessAsync(
                    id, 
                    userId,
                    AccessPermissionLevel.View);
                    
                if (!hasAccess)
                {
                    throw new AccessDeniedException(id, userId, AccessPermissionLevel.View);
                }
                
                // Map to DTO
                return ObjectMapper.Map<Document, DocumentDto>(document);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.View)]
        public virtual async Task<PagedResultDto<DocumentDto>> GetListAsync(GetDocumentListDto input)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Build specifications based on input filters
                var userAccessSpec = new UserAccessDocumentSpecification(
                    userId, 
                    AccessPermissionLevel.View);
                    
                // Create compound specification
                var specifications = new List<DocumentSpecification> { userAccessSpec };
                
                // Add status filter if specified
                if (input.Status.HasValue)
                {
                    // For simplicity, we'll just filter in memory after querying
                }
                
                // Add MIME type filter if specified
                if (!string.IsNullOrWhiteSpace(input.MimeType))
                {
                    specifications.Add(new MimeTypeSpecification(input.MimeType));
                }
                
                // Add uploader filter if specified
                if (input.UploadedByUserId.HasValue)
                {
                    specifications.Add(new UploadedBySpecification(input.UploadedByUserId.Value));
                }
                
                // Add date range filter if specified
                if (input.UploadedFromDate.HasValue && input.UploadedToDate.HasValue)
                {
                    specifications.Add(new UploadDateRangeSpecification(
                        input.UploadedFromDate.Value,
                        input.UploadedToDate.Value));
                }
                
                // Add filename filter if specified
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    specifications.Add(new FilenameLikeSpecification(input.Filter));
                }
                
                // Get documents based on specifications
                // Note: In a real implementation, we'd build a compound specification
                // and pass it to the repository. For simplicity, we'll query with the 
                // first specification and then filter in memory.
                var documents = await _documentRepository.GetListAsync(userAccessSpec);
                
                // Filter documents based on other specifications
                foreach (var spec in specifications.Skip(1))
                {
                    documents = documents.Where(d => spec.IsSatisfiedBy(d)).ToList();
                }
                
                // Filter by status if specified
                if (input.Status.HasValue)
                {
                    documents = documents.Where(d => d.Status == input.Status.Value).ToList();
                }
                
                // Get total count
                var totalCount = documents.Count;
                
                // Apply sorting
                documents = ApplySorting(documents, input.Sorting);
                
                // Apply paging
                documents = documents
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<Document>, List<DocumentDto>>(documents);
                
                return new PagedResultDto<DocumentDto>(totalCount, dtos);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.View)]
        public virtual async Task<PagedResultDto<DocumentVersionDto>> GetVersionsAsync(GetDocumentVersionsDto input)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Check access
                var hasAccess = await _documentManager.CheckDocumentAccessAsync(
                    input.DocumentId, 
                    userId,
                    AccessPermissionLevel.View);
                    
                if (!hasAccess)
                {
                    throw new AccessDeniedException(input.DocumentId, userId, AccessPermissionLevel.View);
                }
                
                // Get document versions
                var document = await _documentRepository.GetDocumentWithVersionsAsync(input.DocumentId);
                var versions = document.Versions.ToList();
                
                // Apply sorting
                versions = versions
                    .OrderByDescending(v => v.VersionNumber.Major)
                    .ThenByDescending(v => v.VersionNumber.Minor)
                    .ToList();
                
                // Get total count
                var totalCount = versions.Count;
                
                // Apply paging
                versions = versions
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<DocumentVersion>, List<DocumentVersionDto>>(versions);
                
                return new PagedResultDto<DocumentVersionDto>(totalCount, dtos);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Create)]
        public virtual async Task<DocumentDto> CreateAsync(UploadDocumentDto input, FileInputContent fileContent)
        {
            try
            {
                var userId = GetCurrentUserId();
                var tenantId = CurrentTenant.Id;
                
                // Validate file content
                if (fileContent == null || fileContent.Content == null)
                {
                    throw new UserFriendlyException(L["Error.FileContentRequired"]);
                }
                
                // Create document using domain service
                var document = await _documentManager.CreateDocumentAsync(
                    fileContent.Content,
                    input.Filename,
                    input.MimeType,
                    userId,
                    tenantId);
                
                // Map to DTO
                return ObjectMapper.Map<Document, DocumentDto>(document);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Edit)]
        public virtual async Task<DocumentVersionDto> CreateVersionAsync(CreateDocumentVersionDto input, FileInputContent fileContent)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Validate file content
                if (fileContent == null || fileContent.Content == null)
                {
                    throw new UserFriendlyException(L["Error.FileContentRequired"]);
                }
                
                // Create version using domain service
                var version = await _documentManager.CreateDocumentVersionAsync(
                    input.DocumentId,
                    fileContent.Content,
                    input.Filename,
                    input.MimeType,
                    input.ChangeComment,
                    userId);
                
                // Map to DTO
                return ObjectMapper.Map<DocumentVersion, DocumentVersionDto>(version);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.View)]
        public virtual async Task<DocumentDownloadLinkDto> GetDownloadLinkAsync(GetDocumentDownloadLinkDto input)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Get document
                var document = await _documentRepository.GetAsync(input.DocumentId);
                
                // Get version if specified
                FileVersionNumber versionNumber = null;
                if (input.VersionNumber.HasValue)
                {
                    versionNumber = FileVersionNumber.Create(input.VersionNumber.Value);
                }
                
                // Get download URL using domain service
                var downloadUrl = await _documentManager.GetDownloadUrlAsync(
                    input.DocumentId,
                    userId,
                    versionNumber);
                
                // Get metadata for response
                string filename;
                string mimeType;
                
                if (versionNumber != null)
                {
                    var version = document.GetVersion(versionNumber);
                    filename = version.Metadata.Filename;
                    mimeType = version.Metadata.MimeType;
                }
                else
                {
                    filename = document.Metadata.Filename;
                    mimeType = document.Metadata.MimeType;
                }
                
                // Create response
                return new DocumentDownloadLinkDto
                {
                    DownloadUrl = downloadUrl,
                    Filename = filename,
                    MimeType = mimeType,
                    ExpiresAt = _clock.Now.AddMinutes(input.ExpirationMinutes)
                };
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.ManageAccess)]
        public virtual async Task GrantAccessAsync(GrantDocumentAccessDto input)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Grant access using domain service
                await _documentManager.GrantDocumentAccessAsync(
                    input.DocumentId,
                    input.PrincipalId,
                    input.IsRole,
                    input.PermissionLevel,
                    userId);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.ManageAccess)]
        public virtual async Task RevokeAccessAsync(RevokeDocumentAccessDto input)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Revoke access using domain service
                await _documentManager.RevokeDocumentAccessAsync(
                    input.DocumentId,
                    input.PrincipalId,
                    input.IsRole,
                    userId);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Edit)]
        public virtual async Task ArchiveAsync(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Archive document using domain service
                await _documentManager.ArchiveDocumentAsync(id, userId);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Edit)]
        public virtual async Task RestoreAsync(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Restore document using domain service
                await _documentManager.RestoreDocumentAsync(id, userId);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Delete)]
        public virtual async Task MarkForDeletionAsync(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Get document
                var document = await _documentRepository.GetAsync(id);
                
                // Mark for deletion
                document.MarkForDeletion(userId, _clock.Now);
                
                // Save changes
                await _documentRepository.UpdateAsync(document);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        [Authorize(DocumentManagementPermissions.Documents.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Delete document using domain service
                await _documentManager.DeleteDocumentAsync(id, true, userId);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error.DefaultMessage"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error.DefaultMessage"]);
            }
        }

        #region Helper Methods

        private Guid GetCurrentUserId()
        {
            if (_currentUser.Id == null)
            {
                throw new UserFriendlyException(L["Error.UserNotAuthenticated"]);
            }
            
            return _currentUser.Id.Value;
        }
        
        private List<Document> ApplySorting(List<Document> documents, string sorting)
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return documents.OrderByDescending(d => d.CreationTime).ToList();
            }
            
            // For simplicity, we'll just handle a few sorting options
            if (sorting.Contains("Filename", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.StartsWith("-") 
                    ? documents.OrderByDescending(d => d.Metadata.Filename).ToList()
                    : documents.OrderBy(d => d.Metadata.Filename).ToList();
            }
            
            if (sorting.Contains("UploadedAt", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.StartsWith("-") 
                    ? documents.OrderByDescending(d => d.Metadata.UploadedAt).ToList()
                    : documents.OrderBy(d => d.Metadata.UploadedAt).ToList();
            }
            
            if (sorting.Contains("FileSize", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.StartsWith("-") 
                    ? documents.OrderByDescending(d => d.Metadata.FileSize).ToList()
                    : documents.OrderBy(d => d.Metadata.FileSize).ToList();
            }
            
            // Default to creation time
            return documents.OrderByDescending(d => d.CreationTime).ToList();
        }
        
        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in DocumentAppService: {ErrorMessage}", 
                exception.Message);
                
            await Task.CompletedTask;
        }

        #endregion
    }
}
