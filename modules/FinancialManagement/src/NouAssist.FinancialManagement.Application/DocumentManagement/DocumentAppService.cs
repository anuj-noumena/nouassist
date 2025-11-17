using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.DocumentManagement.Dtos;
using NouAssist.DocumentManagement.Permissions;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.Data;
using NouAssist.FinancialManagement.DocumentManagement.Services;
using NouAssist.FinancialManagement.DocumentManagement.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DocumentManagement
{
    [Authorize(DocumentManagementPermissions.Documents.View)]
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly DocumentStorageService _storageService;
        private readonly DocumentOCRService _ocrService;
        private readonly DocumentAccessControlService _accessControlService;

        public DocumentAppService(
            IDocumentRepository documentRepository,
            DocumentStorageService storageService,
            DocumentOCRService ocrService,
            DocumentAccessControlService accessControlService)
        {
            _documentRepository = documentRepository;
            _storageService = storageService;
            _ocrService = ocrService;
            _accessControlService = accessControlService;
        }

        public async Task<DocumentDto> GetAsync(Guid id)
        {
            // Check access
            var canAccess = await _accessControlService.CanAccessDocumentAsync(
                id,
                CurrentUser.Id.Value,
                await IsAdminAsync());

            if (!canAccess)
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("Access denied to this document");
            }

            var document = await _documentRepository.GetAsync(id);
            return ObjectMapper.Map<Document, DocumentDto>(document);
        }

        public async Task<PagedResultDto<DocumentDto>> SearchAsync(DocumentSearchDto input)
        {
            var documents = await _documentRepository.SearchAsync(
                input.SearchTerm,
                input.Type,
                input.UploadedAfter,
                input.UploadedBefore,
                input.MaxResultCount);

            // Filter by access control
            var accessibleDocuments = new List<Document>();
            foreach (var doc in documents)
            {
                if (await _accessControlService.CanAccessDocumentAsync(doc.Id, CurrentUser.Id.Value, await IsAdminAsync()))
                {
                    accessibleDocuments.Add(doc);
                }
            }

            return new PagedResultDto<DocumentDto>(
                accessibleDocuments.Count,
                ObjectMapper.Map<List<Document>, List<DocumentDto>>(accessibleDocuments));
        }

        public async Task<ListResultDto<DocumentDto>> GetByEntityAsync(string entityType, string entityId)
        {
            var documents = await _documentRepository.GetByEntityReferenceAsync(entityType, entityId);

            return new ListResultDto<DocumentDto>(
                ObjectMapper.Map<List<Document>, List<DocumentDto>>(documents));
        }

        [Authorize(DocumentManagementPermissions.Documents.Upload)]
        public async Task<DocumentDto> UploadAsync(CreateDocumentDto input)
        {
            EntityReference entityReference = null;
            if (!string.IsNullOrEmpty(input.EntityType) && !string.IsNullOrEmpty(input.EntityId))
            {
                entityReference = new EntityReference(input.EntityType, input.EntityId);
            }

            var document = await _storageService.CreateDocumentAsync(
                CurrentTenant.Id,
                input.FileName,
                input.ContentType,
                input.FileContent.Length,
                input.FileContent,
                input.Type,
                entityReference,
                input.Description);

            return ObjectMapper.Map<Document, DocumentDto>(document);
        }

        [Authorize(DocumentManagementPermissions.Documents.Update)]
        public async Task<DocumentDto> UpdateAsync(Guid id, UpdateDocumentDto input)
        {
            var document = await _documentRepository.GetAsync(id);

            if (!document.IsOwnedBy(CurrentUser.Id.Value) && !await IsAdminAsync())
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("Only the owner can update this document");
            }

            if (!string.IsNullOrEmpty(input.Description))
            {
                document.Description = input.Description;
            }

            if (input.AccessLevel.HasValue)
            {
                document.ChangeAccessLevel(input.AccessLevel.Value);
            }

            if (input.Tags != null)
            {
                foreach (var tag in input.Tags)
                {
                    document.AddTag(tag.Key, tag.Value);
                }
            }

            await _documentRepository.UpdateAsync(document);

            return ObjectMapper.Map<Document, DocumentDto>(document);
        }

        [Authorize(DocumentManagementPermissions.Documents.Download)]
        public async Task<DocumentDownloadDto> DownloadAsync(Guid id)
        {
            var canAccess = await _accessControlService.CanAccessDocumentAsync(
                id,
                CurrentUser.Id.Value,
                await IsAdminAsync());

            if (!canAccess)
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("Access denied to this document");
            }

            var document = await _documentRepository.GetAsync(id);

            // Record access
            document.RecordAccess();
            await _documentRepository.UpdateAsync(document);

            // In real implementation, retrieve from blob storage
            // For now, return placeholder
            return new DocumentDownloadDto
            {
                FileContent = new byte[0],
                FileName = document.Metadata.FileName,
                ContentType = document.Metadata.ContentType
            };
        }

        [Authorize(DocumentManagementPermissions.Documents.Delete)]
        public async Task DeleteAsync(Guid id, string reason = null)
        {
            await _storageService.DeleteDocumentAsync(id, CurrentUser.Id.Value, reason);
        }

        [Authorize(DocumentManagementPermissions.Documents.Archive)]
        public async Task ArchiveAsync(Guid id)
        {
            var document = await _documentRepository.GetAsync(id);
            document.Archive();
            await _documentRepository.UpdateAsync(document);
        }

        [Authorize(DocumentManagementPermissions.Documents.ProcessOCR)]
        public async Task<int> ProcessPendingOCRAsync()
        {
            var documents = await _ocrService.GetDocumentsForOCRAsync();
            
            foreach (var document in documents)
            {
                await _ocrService.ProcessDocumentAsync(document.Id);
            }

            return documents.Count;
        }

        private async Task<bool> IsAdminAsync()
        {
            return await AuthorizationService.IsGrantedAsync("Admin");
        }
    }
}
