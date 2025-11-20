using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.Exceptions;
using NouAssist.BusinessOperations.DocumentManagement.Interfaces.Services;
using NouAssist.BusinessOperations.DocumentManagement.Repositories;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.DocumentManagement.Services
{
    /// <summary>
    /// Domain service for document management operations
    /// </summary>
    public class DocumentManager : DomainService, IDocumentManager
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentStorageService _documentStorageService;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly ILogger<DocumentManager> _logger;

        public DocumentManager(
            IDocumentRepository documentRepository,
            IDocumentStorageService documentStorageService,
            ICurrentUser currentUser,
            IClock clock,
            ILogger<DocumentManager> logger)
        {
            _documentRepository = documentRepository;
            _documentStorageService = documentStorageService;
            _currentUser = currentUser;
            _clock = clock;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Document> CreateDocumentAsync(
            Stream content,
            string filename,
            string mimeType,
            Guid userId,
            Guid? tenantId = null)
        {
            try
            {
                // Validate content stream
                if (content == null || content.Length == 0)
                {
                    throw new ArgumentException("Content stream cannot be null or empty", nameof(content));
                }

                // Upload document to storage
                var storageReference = await _documentStorageService.UploadAsync(
                    content, 
                    filename, 
                    mimeType, 
                    tenantId);

                // Create document metadata
                var metadata = new DocumentMetadata(
                    filename,
                    mimeType,
                    content.Length,
                    userId,
                    _clock.Now);

                // Create document
                var documentId = GuidGenerator.Create();
                var document = Document.Create(
                    documentId,
                    metadata,
                    storageReference,
                    tenantId);

                // Save document
                await _documentRepository.InsertAsync(document);

                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create document {Filename}", filename);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DocumentVersion> CreateDocumentVersionAsync(
            Guid documentId,
            Stream content,
            string filename,
            string mimeType,
            string changeComment,
            Guid userId)
        {
            try
            {
                // Validate content stream
                if (content == null || content.Length == 0)
                {
                    throw new ArgumentException("Content stream cannot be null or empty", nameof(content));
                }

                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Upload document to storage
                var storageReference = await _documentStorageService.UploadAsync(
                    content, 
                    filename, 
                    mimeType, 
                    document.TenantId);

                // Create document metadata
                var metadata = new DocumentMetadata(
                    filename,
                    mimeType,
                    content.Length,
                    userId,
                    _clock.Now);

                // Create new version
                var version = document.CreateNewVersion(
                    metadata,
                    storageReference,
                    changeComment,
                    userId,
                    _clock.Now);

                // Save document
                await _documentRepository.UpdateAsync(document);

                return version;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create document version for document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetDownloadUrlAsync(
            Guid documentId,
            Guid userId,
            FileVersionNumber versionNumber = null)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Check access
                var hasAccess = await CheckDocumentAccessAsync(
                    documentId,
                    userId,
                    AccessPermissionLevel.Download);

                if (!hasAccess)
                {
                    throw new AccessDeniedException(documentId, userId, AccessPermissionLevel.Download);
                }

                // Get storage reference for the requested version or current version
                StorageReference storageReference;
                if (versionNumber != null)
                {
                    var version = document.GetVersion(versionNumber);
                    storageReference = version.StorageReference;
                }
                else
                {
                    storageReference = document.StorageReference;
                }

                // Get download URL
                return await _documentStorageService.GetDownloadUrlAsync(
                    storageReference,
                    TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get download URL for document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task GrantDocumentAccessAsync(
            Guid documentId,
            Guid principalId,
            bool isRole,
            AccessPermissionLevel permissionLevel,
            Guid grantedBy)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Grant access
                document.GrantAccess(
                    principalId,
                    isRole,
                    permissionLevel,
                    grantedBy,
                    _clock.Now);

                // Save document
                await _documentRepository.UpdateAsync(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to grant access to document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RevokeDocumentAccessAsync(
            Guid documentId,
            Guid principalId,
            bool isRole,
            Guid revokedBy)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Revoke access
                document.RevokeAccess(
                    principalId,
                    isRole,
                    revokedBy,
                    _clock.Now);

                // Save document
                await _documentRepository.UpdateAsync(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke access from document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteDocumentAsync(
            Guid documentId,
            bool physically,
            Guid userId)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Delete document
                document.Delete(physically, userId, _clock.Now);

                if (physically)
                {
                    // Delete all versions from storage
                    foreach (var version in document.Versions)
                    {
                        await _documentStorageService.DeleteAsync(version.StorageReference);
                    }

                    // Delete document from repository
                    await _documentRepository.DeleteAsync(document);
                }
                else
                {
                    // Update document status
                    await _documentRepository.UpdateAsync(document);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ArchiveDocumentAsync(Guid documentId, Guid userId)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Archive document
                document.Archive(userId, _clock.Now);

                // Save document
                await _documentRepository.UpdateAsync(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to archive document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RestoreDocumentAsync(Guid documentId, Guid userId)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Restore document
                document.Restore(userId, _clock.Now);

                // Save document
                await _documentRepository.UpdateAsync(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to restore document {DocumentId}", documentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CheckDocumentAccessAsync(
            Guid documentId,
            Guid userId,
            AccessPermissionLevel requiredLevel)
        {
            try
            {
                // Get document
                var document = await _documentRepository.GetAsync(documentId);

                // Get user roles
                var roleIds = GetUserRoleIds(userId);

                // Check access
                return document.HasAccess(userId, roleIds, requiredLevel);
            }
            catch (DocumentNotFoundException)
            {
                // Document not found - no access
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check access for document {DocumentId}", documentId);
                throw;
            }
        }

        private List<Guid> GetUserRoleIds(Guid userId)
        {
            // In a real implementation, this would get the user's roles from identity system
            // For simplicity, we'll return an empty list
            return new List<Guid>();
        }
    }
}
