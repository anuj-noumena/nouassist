using System;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Data;
using NouAssist.DocumentManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.DocumentManagement.Services
{
    public class DocumentAccessControlService : DomainService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentAccessControlService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<bool> CanAccessDocumentAsync(Guid documentId, Guid userId, bool isAdmin = false)
        {
            var document = await _documentRepository.GetAsync(documentId);

            if (document == null)
            {
                return false;
            }

            // Admins can access everything
            if (isAdmin)
            {
                return true;
            }

            // Owner can always access
            if (document.IsOwnedBy(userId))
            {
                return true;
            }

            // Check access level
            return document.AccessLevel switch
            {
                AccessLevel.Public => true,
                AccessLevel.Shared => await IsSharedWithUserAsync(documentId, userId),
                AccessLevel.Private => false,
                AccessLevel.Restricted => await HasExplicitPermissionAsync(documentId, userId),
                _ => false
            };
        }

        public async Task GrantAccessAsync(Guid documentId, Guid userId, Guid grantedBy)
        {
            var document = await _documentRepository.GetAsync(documentId);

            if (document == null)
            {
                throw new BusinessException("DocumentManagement:DocumentNotFound")
                    .WithData("DocumentId", documentId);
            }

            // Check if granter has permission to grant access
            if (!document.IsOwnedBy(grantedBy))
            {
                throw new BusinessException("DocumentManagement:AccessDenied");
            }

            // Implementation would store access grant in a separate entity
            // For simplicity, this is a placeholder
            await Task.CompletedTask;
        }

        public async Task RevokeAccessAsync(Guid documentId, Guid userId, Guid revokedBy)
        {
            var document = await _documentRepository.GetAsync(documentId);

            if (document == null)
            {
                throw new BusinessException("DocumentManagement:DocumentNotFound")
                    .WithData("DocumentId", documentId);
            }

            // Check if revoker has permission
            if (!document.IsOwnedBy(revokedBy))
            {
                throw new BusinessException("DocumentManagement:AccessDenied");
            }

            // Implementation would revoke access in separate entity
            await Task.CompletedTask;
        }

        private async Task<bool> IsSharedWithUserAsync(Guid documentId, Guid userId)
        {
            // Check if document is shared with user
            // Implementation would query document sharing entity
            await Task.CompletedTask;
            return false;
        }

        private async Task<bool> HasExplicitPermissionAsync(Guid documentId, Guid userId)
        {
            // Check if user has explicit permission
            await Task.CompletedTask;
            return false;
        }
    }
}
