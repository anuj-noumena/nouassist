using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Data;
using NouAssist.FinancialManagement.DocumentManagement.Dtos;
using NouAssist.FinancialManagement.DocumentManagement.Permissions;
using NouAssist.FinancialManagement.DocumentManagement.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.DocumentManagement
{
    [Authorize(DocumentManagementPermissions.Folders.View)]
    public class DocumentFolderAppService : ApplicationService, IDocumentFolderAppService
    {
        private readonly IDocumentFolderRepository _folderRepository;
        private readonly DocumentFolderService _folderService;

        public DocumentFolderAppService(
            IDocumentFolderRepository folderRepository,
            DocumentFolderService folderService)
        {
            _folderRepository = folderRepository;
            _folderService = folderService;
        }

        public async Task<DocumentFolderDto> GetAsync(Guid id)
        {
            var folder = await _folderRepository.GetAsync(id);
            return ObjectMapper.Map<DocumentFolder, DocumentFolderDto>(folder);
        }

        public async Task<ListResultDto<DocumentFolderDto>> GetRootFoldersAsync()
        {
            var folders = await _folderRepository.GetRootFoldersAsync(CurrentTenant.Id);
            return new ListResultDto<DocumentFolderDto>(
                ObjectMapper.Map<List<DocumentFolder>, List<DocumentFolderDto>>(folders));
        }

        public async Task<ListResultDto<DocumentFolderDto>> GetChildFoldersAsync(Guid parentId)
        {
            var folders = await _folderRepository.GetChildFoldersAsync(parentId);
            return new ListResultDto<DocumentFolderDto>(
                ObjectMapper.Map<List<DocumentFolder>, List<DocumentFolderDto>>(folders));
        }

        [Authorize(DocumentManagementPermissions.Folders.Create)]
        public async Task<DocumentFolderDto> CreateAsync(CreateDocumentFolderDto input)
        {
            var folder = await _folderService.CreateFolderAsync(
                CurrentTenant.Id,
                input.Name,
                input.ParentFolderId,
                input.Description);

            return ObjectMapper.Map<DocumentFolder, DocumentFolderDto>(folder);
        }

        [Authorize(DocumentManagementPermissions.Folders.Update)]
        public async Task<DocumentFolderDto> UpdateAsync(Guid id, CreateDocumentFolderDto input)
        {
            var folder = await _folderRepository.GetAsync(id);

            if (!string.IsNullOrEmpty(input.Name) && folder.Name != input.Name)
            {
                folder.Rename(input.Name);
            }

            if (!string.IsNullOrEmpty(input.Description))
            {
                folder.Description = input.Description;
            }

            await _folderRepository.UpdateAsync(folder);

            return ObjectMapper.Map<DocumentFolder, DocumentFolderDto>(folder);
        }

        [Authorize(DocumentManagementPermissions.Folders.Manage)]
        public async Task MoveAsync(Guid id, Guid? newParentId)
        {
            await _folderService.MoveFolderAsync(id, newParentId);
        }

        [Authorize(DocumentManagementPermissions.Folders.Manage)]
        public async Task AddDocumentAsync(Guid folderId, Guid documentId)
        {
            await _folderService.AddDocumentToFolderAsync(folderId, documentId);
        }

        [Authorize(DocumentManagementPermissions.Folders.Manage)]
        public async Task RemoveDocumentAsync(Guid folderId, Guid documentId)
        {
            await _folderService.RemoveDocumentFromFolderAsync(folderId, documentId);
        }

        [Authorize(DocumentManagementPermissions.Folders.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var folder = await _folderRepository.GetAsync(id);

            // Check if folder has documents
            if (folder.DocumentIds.Count > 0)
            {
                throw new Volo.Abp.BusinessException("DocumentManagement:CannotDeleteFolderWithDocuments");
            }

            // Check if folder has children
            var children = await _folderRepository.GetChildFoldersAsync(id);
            if (children.Count > 0)
            {
                throw new Volo.Abp.BusinessException("DocumentManagement:CannotDeleteFolderWithChildren");
            }

            await _folderRepository.DeleteAsync(id);
        }
    }
}
