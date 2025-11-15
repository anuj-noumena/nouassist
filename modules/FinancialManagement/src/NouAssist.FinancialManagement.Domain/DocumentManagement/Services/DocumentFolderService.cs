using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Data;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.DocumentManagement.Services
{
    public class DocumentFolderService : DomainService
    {
        private readonly IDocumentFolderRepository _folderRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocumentFolderService(
            IDocumentFolderRepository folderRepository,
            IDocumentRepository documentRepository)
        {
            _folderRepository = folderRepository;
            _documentRepository = documentRepository;
        }

        public async Task<DocumentFolder> CreateFolderAsync(
            Guid? tenantId,
            string name,
            Guid? parentFolderId = null,
            string description = null)
        {
            string parentPath = null;

            if (parentFolderId.HasValue)
            {
                var parentFolder = await _folderRepository.GetAsync(parentFolderId.Value);
                if (parentFolder == null)
                {
                    throw new BusinessException("DocumentManagement:ParentFolderNotFound");
                }
                parentPath = parentFolder.Path;
            }

            var folder = new DocumentFolder(
                GuidGenerator.Create(),
                tenantId,
                name,
                parentFolderId,
                parentPath,
                description);

            await _folderRepository.InsertAsync(folder);

            return folder;
        }

        public async Task MoveFolderAsync(Guid folderId, Guid? newParentFolderId)
        {
            var folder = await _folderRepository.GetAsync(folderId);

            if (folder == null)
            {
                throw new BusinessException("DocumentManagement:FolderNotFound");
            }

            // Prevent moving to self or descendant
            if (newParentFolderId.HasValue)
            {
                if (newParentFolderId.Value == folderId)
                {
                    throw new BusinessException("DocumentManagement:CannotMoveToSelf");
                }

                if (await IsDescendantAsync(folderId, newParentFolderId.Value))
                {
                    throw new BusinessException("DocumentManagement:CannotMoveToDescendant");
                }
            }

            string newParentPath = null;
            if (newParentFolderId.HasValue)
            {
                var newParentFolder = await _folderRepository.GetAsync(newParentFolderId.Value);
                newParentPath = newParentFolder.Path;
            }

            folder.Move(newParentFolderId, newParentPath);
            await _folderRepository.UpdateAsync(folder);

            // Update all child folders' paths
            await UpdateChildFolderPathsAsync(folder);
        }

        public async Task AddDocumentToFolderAsync(Guid folderId, Guid documentId)
        {
            var folder = await _folderRepository.GetAsync(folderId);
            var document = await _documentRepository.GetAsync(documentId);

            if (folder == null || document == null)
            {
                throw new BusinessException("DocumentManagement:FolderOrDocumentNotFound");
            }

            folder.AddDocument(documentId);
            await _folderRepository.UpdateAsync(folder);
        }

        public async Task RemoveDocumentFromFolderAsync(Guid folderId, Guid documentId)
        {
            var folder = await _folderRepository.GetAsync(folderId);

            if (folder == null)
            {
                throw new BusinessException("DocumentManagement:FolderNotFound");
            }

            folder.RemoveDocument(documentId);
            await _folderRepository.UpdateAsync(folder);
        }

        private async Task<bool> IsDescendantAsync(Guid ancestorId, Guid potentialDescendantId)
        {
            var potentialDescendant = await _folderRepository.GetAsync(potentialDescendantId);

            if (potentialDescendant == null)
            {
                return false;
            }

            var currentParentId = potentialDescendant.ParentFolderId;

            while (currentParentId.HasValue)
            {
                if (currentParentId.Value == ancestorId)
                {
                    return true;
                }

                var parent = await _folderRepository.GetAsync(currentParentId.Value);
                currentParentId = parent?.ParentFolderId;
            }

            return false;
        }

        private async Task UpdateChildFolderPathsAsync(DocumentFolder parentFolder)
        {
            var childFolders = await _folderRepository.GetChildFoldersAsync(parentFolder.Id);

            foreach (var child in childFolders)
            {
                child.Move(parentFolder.Id, parentFolder.Path);
                await _folderRepository.UpdateAsync(child);

                // Recursively update descendants
                await UpdateChildFolderPathsAsync(child);
            }
        }
    }
}
