using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.Events;
using NouAssist.BusinessOperations.DocumentManagement.Exceptions;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.DocumentManagement.Aggregates
{
    /// <summary>
    /// Document aggregate root that represents a file with its metadata, versions and access control
    /// </summary>
    public class Document : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public DocumentMetadata Metadata { get; private set; }
        
        public StorageReference StorageReference { get; private set; }
        
        public DocumentStatus Status { get; private set; }
        
        public FileVersionNumber CurrentVersionNumber { get; private set; }
        
        private readonly List<DocumentVersion> _versions;
        public IReadOnlyCollection<DocumentVersion> Versions => _versions.AsReadOnly();
        
        private readonly List<AccessGrant> _accessGrants;
        public IReadOnlyCollection<AccessGrant> AccessGrants => _accessGrants.AsReadOnly();

        protected Document()
        {
            _versions = new List<DocumentVersion>();
            _accessGrants = new List<AccessGrant>();
        }

        /// <summary>
        /// Creates a new document
        /// </summary>
        public static Document Create(
            Guid id,
            DocumentMetadata metadata,
            StorageReference storageReference,
            Guid? tenantId = null)
        {
            return new Document(id, metadata, storageReference, tenantId);
        }

        private Document(
            Guid id,
            DocumentMetadata metadata, 
            StorageReference storageReference,
            Guid? tenantId = null) : base(id)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            StorageReference = storageReference ?? throw new ArgumentNullException(nameof(storageReference));
            Status = DocumentStatus.Active;
            CurrentVersionNumber = FileVersionNumber.Initial;
            TenantId = tenantId;
            
            _versions = new List<DocumentVersion>();
            _accessGrants = new List<AccessGrant>();
            
            // Add initial version
            _versions.Add(CreateVersion(CurrentVersionNumber, metadata, storageReference, "Initial version"));
            
            // Grant owner access to the creator
            GrantAccess(
                metadata.UploadedByUserId,
                false,
                AccessPermissionLevel.Owner,
                metadata.UploadedByUserId,
                metadata.UploadedAt);
                
            // Raise domain event
            AddDistributedEvent(new DocumentCreatedEto
            {
                Id = Id,
                Filename = metadata.Filename,
                MimeType = metadata.MimeType,
                FileSize = metadata.FileSize,
                CreatorId = metadata.UploadedByUserId,
                CreationTime = metadata.UploadedAt,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Creates a new version of the document
        /// </summary>
        public DocumentVersion CreateNewVersion(
            DocumentMetadata metadata,
            StorageReference storageReference,
            string changeComment,
            Guid userId,
            DateTime timestamp)
        {
            CheckDocumentIsActive();
            CheckAccess(userId, AccessPermissionLevel.Edit);
            
            var newVersionNumber = CurrentVersionNumber.Next();
            CurrentVersionNumber = newVersionNumber;
            
            var version = CreateVersion(newVersionNumber, metadata, storageReference, changeComment);
            _versions.Add(version);
            
            // Update the current metadata and storage reference
            Metadata = metadata;
            StorageReference = storageReference;
            
            // Raise domain event
            AddDistributedEvent(new DocumentVersionCreatedEto
            {
                DocumentId = Id,
                VersionNumber = newVersionNumber.ToVersionNumber(),
                Filename = metadata.Filename,
                FileSize = metadata.FileSize,
                CreatorId = userId,
                CreationTime = timestamp,
                TenantId = TenantId
            });
            
            return version;
        }

        /// <summary>
        /// Grants access to the document for a user or role
        /// </summary>
        public void GrantAccess(
            Guid principalId,
            bool isRole,
            AccessPermissionLevel permissionLevel,
            Guid grantedBy,
            DateTime timestamp)
        {
            CheckDocumentIsActive();
            
            // If not initial creation, check permissions
            if (grantedBy != Metadata.UploadedByUserId)
            {
                CheckAccess(grantedBy, AccessPermissionLevel.ManageAccess);
            }
            
            var existingGrant = _accessGrants.FirstOrDefault(g => 
                g.PrincipalId == principalId && g.IsRole == isRole);
                
            if (existingGrant != null)
            {
                // Update existing grant
                _accessGrants.Remove(existingGrant);
                _accessGrants.Add(existingGrant.UpdatePermissionLevel(permissionLevel, grantedBy, timestamp));
            }
            else
            {
                // Add new grant
                _accessGrants.Add(new AccessGrant(
                    principalId,
                    isRole,
                    permissionLevel,
                    grantedBy,
                    timestamp
                ));
            }
            
            // Raise domain event
            AddDistributedEvent(new DocumentAccessGrantedEto
            {
                DocumentId = Id,
                PrincipalId = principalId,
                IsRole = isRole,
                PermissionLevel = permissionLevel,
                GrantedBy = grantedBy,
                GrantedAt = timestamp,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Revokes access to the document from a user or role
        /// </summary>
        public void RevokeAccess(Guid principalId, bool isRole, Guid revokedBy, DateTime timestamp)
        {
            CheckDocumentIsActive();
            CheckAccess(revokedBy, AccessPermissionLevel.ManageAccess);
            
            // Don't allow revoking owner access from the creator
            var existingGrant = _accessGrants.FirstOrDefault(g => 
                g.PrincipalId == principalId && g.IsRole == isRole);
                
            if (existingGrant == null)
            {
                return; // No access to revoke
            }
            
            if (existingGrant.PermissionLevel == AccessPermissionLevel.Owner && 
                principalId == Metadata.UploadedByUserId)
            {
                throw new InvalidDocumentOperationException(
                    Id,
                    "RevokeAccess",
                    "Cannot revoke owner access from the document creator");
            }
            
            _accessGrants.Remove(existingGrant);
        }
        
        /// <summary>
        /// Marks the document for deletion
        /// </summary>
        public void MarkForDeletion(Guid userId, DateTime timestamp)
        {
            if (Status == DocumentStatus.Deleted || Status == DocumentStatus.MarkedForDeletion)
            {
                throw new InvalidDocumentOperationException(
                    Id,
                    "MarkForDeletion",
                    "Document is already marked for deletion or deleted");
            }
            
            CheckAccess(userId, AccessPermissionLevel.Delete);
            
            Status = DocumentStatus.MarkedForDeletion;
        }
        
        /// <summary>
        /// Permanently deletes the document
        /// </summary>
        public void Delete(bool physically, Guid userId, DateTime timestamp)
        {
            CheckAccess(userId, AccessPermissionLevel.Delete);
            
            Status = DocumentStatus.Deleted;
            
            // Raise domain event
            AddDistributedEvent(new DocumentDeletedEto
            {
                Id = Id,
                IsPhysicallyDeleted = physically,
                DeletedBy = userId,
                DeletionTime = timestamp,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Archives the document
        /// </summary>
        public void Archive(Guid userId, DateTime timestamp)
        {
            CheckDocumentIsActive();
            CheckAccess(userId, AccessPermissionLevel.Edit);
            
            Status = DocumentStatus.Archived;
        }
        
        /// <summary>
        /// Restores the document from archived or marked for deletion status
        /// </summary>
        public void Restore(Guid userId, DateTime timestamp)
        {
            if (Status == DocumentStatus.Deleted)
            {
                throw new InvalidDocumentOperationException(
                    Id,
                    "Restore",
                    "Cannot restore a permanently deleted document");
            }
            
            if (Status == DocumentStatus.Active)
            {
                throw new InvalidDocumentOperationException(
                    Id,
                    "Restore",
                    "Document is already active");
            }
            
            CheckAccess(userId, AccessPermissionLevel.Edit);
            
            Status = DocumentStatus.Active;
        }
        
        /// <summary>
        /// Gets a specific version of the document
        /// </summary>
        public DocumentVersion GetVersion(FileVersionNumber versionNumber)
        {
            var version = _versions.FirstOrDefault(v => v.VersionNumber.Equals(versionNumber));
            
            if (version == null)
            {
                throw new DocumentManagementDomainException("DocumentManagement:ErrorMessage:DocumentVersionNotFound")
                    .WithData("documentId", Id)
                    .WithData("versionNumber", versionNumber.ToString());
            }
            
            return version;
        }
        
        /// <summary>
        /// Checks if a principal has the required access level
        /// </summary>
        public bool HasAccess(Guid principalId, List<Guid> roleIds, AccessPermissionLevel requiredLevel)
        {
            // Check direct user access
            var userGrant = _accessGrants.FirstOrDefault(g => 
                g.PrincipalId == principalId && !g.IsRole);
                
            if (userGrant != null && userGrant.PermissionLevel >= requiredLevel)
            {
                return true;
            }
            
            // Check role access
            if (roleIds != null && roleIds.Any())
            {
                foreach (var roleId in roleIds)
                {
                    var roleGrant = _accessGrants.FirstOrDefault(g => 
                        g.PrincipalId == roleId && g.IsRole);
                        
                    if (roleGrant != null && roleGrant.PermissionLevel >= requiredLevel)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        private void CheckDocumentIsActive()
        {
            if (Status != DocumentStatus.Active)
            {
                throw new InvalidDocumentOperationException(
                    Id,
                    "Operation",
                    $"Document is not in Active status. Current status: {Status}");
            }
        }
        
        private void CheckAccess(Guid userId, AccessPermissionLevel requiredLevel)
        {
            var userGrant = _accessGrants.FirstOrDefault(g => 
                g.PrincipalId == userId && !g.IsRole);
                
            if (userGrant == null || userGrant.PermissionLevel < requiredLevel)
            {
                throw new AccessDeniedException(Id, userId, requiredLevel);
            }
        }
        
        private DocumentVersion CreateVersion(
            FileVersionNumber versionNumber,
            DocumentMetadata metadata,
            StorageReference storageReference,
            string changeComment)
        {
            return new DocumentVersion(
                Id,
                versionNumber,
                metadata,
                storageReference,
                changeComment,
                TenantId
            );
        }
    }
}
