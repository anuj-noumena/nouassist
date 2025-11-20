using System;
using System.IO;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;

namespace NouAssist.BusinessOperations.DocumentManagement.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for document management operations
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Creates a new document
        /// </summary>
        Task<Document> CreateDocumentAsync(
            Stream content,
            string filename,
            string mimeType,
            Guid userId,
            Guid? tenantId = null);
        
        /// <summary>
        /// Creates a new version of a document
        /// </summary>
        Task<DocumentVersion> CreateDocumentVersionAsync(
            Guid documentId,
            Stream content,
            string filename,
            string mimeType,
            string changeComment,
            Guid userId);
        
        /// <summary>
        /// Gets a download URL for a document
        /// </summary>
        Task<string> GetDownloadUrlAsync(
            Guid documentId,
            Guid userId,
            FileVersionNumber versionNumber = null);
        
        /// <summary>
        /// Grants access to a document
        /// </summary>
        Task GrantDocumentAccessAsync(
            Guid documentId,
            Guid principalId,
            bool isRole,
            AccessPermissionLevel permissionLevel,
            Guid grantedBy);
        
        /// <summary>
        /// Revokes access to a document
        /// </summary>
        Task RevokeDocumentAccessAsync(
            Guid documentId,
            Guid principalId,
            bool isRole,
            Guid revokedBy);
        
        /// <summary>
        /// Deletes a document
        /// </summary>
        Task DeleteDocumentAsync(
            Guid documentId,
            bool physically,
            Guid userId);
        
        /// <summary>
        /// Archives a document
        /// </summary>
        Task ArchiveDocumentAsync(
            Guid documentId,
            Guid userId);
        
        /// <summary>
        /// Restores a document from archived or marked for deletion status
        /// </summary>
        Task RestoreDocumentAsync(
            Guid documentId,
            Guid userId);
        
        /// <summary>
        /// Checks if a user has access to a document
        /// </summary>
        Task<bool> CheckDocumentAccessAsync(
            Guid documentId,
            Guid userId,
            AccessPermissionLevel requiredLevel);
    }
}
