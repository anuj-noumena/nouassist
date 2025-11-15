using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Data;
using NouAssist.DocumentManagement.Enums;
using NouAssist.DocumentManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.DocumentManagement.Services
{
    public class DocumentStorageService : DomainService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentStorageService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Document> CreateDocumentAsync(
            Guid? tenantId,
            string fileName,
            string contentType,
            long fileSize,
            byte[] fileContent,
            DocumentType documentType,
            EntityReference entityReference = null,
            string description = null)
        {
            // Calculate file hash for deduplication
            var fileHash = CalculateFileHash(fileContent);

            // Check for duplicate
            var existingDocument = await _documentRepository.FindByHashAsync(fileHash);
            if (existingDocument != null)
            {
                // Return existing document reference (deduplication)
                return existingDocument;
            }

            var metadata = new FileMetadata(fileName, contentType, fileSize, fileHash);
            
            // Generate storage path
            var container = DetermineContainer(documentType);
            var blobPath = GenerateBlobPath(tenantId, fileName);
            var storageLocation = new StorageLocation(container, blobPath);

            var document = new Document(
                GuidGenerator.Create(),
                tenantId,
                metadata,
                storageLocation,
                documentType,
                entityReference,
                AccessLevel.Private,
                description);

            // The actual file upload to blob storage would happen here
            // await _blobStorageService.UploadAsync(storageLocation, fileContent);

            document.MarkAsAvailable();
            
            await _documentRepository.InsertAsync(document);

            return document;
        }

        public async Task DeleteDocumentAsync(Guid documentId, Guid deletedBy, string reason = null)
        {
            var document = await _documentRepository.GetAsync(documentId);

            if (document == null)
            {
                throw new BusinessException("DocumentManagement:DocumentNotFound")
                    .WithData("DocumentId", documentId);
            }

            // Check if document is referenced by other entities
            var hasReferences = await _documentRepository.HasReferencesAsync(documentId);
            if (hasReferences)
            {
                throw new BusinessException("DocumentManagement:CannotDeleteReferenced");
            }

            document.Delete(deletedBy, reason);

            // The actual blob deletion would happen here
            // await _blobStorageService.DeleteAsync(document.StorageLocation);

            await _documentRepository.UpdateAsync(document);
        }

        private string CalculateFileHash(byte[] fileContent)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(fileContent);
            return Convert.ToBase64String(hashBytes);
        }

        private string DetermineContainer(DocumentType documentType)
        {
            return documentType switch
            {
                DocumentType.Receipt => "receipts",
                DocumentType.Invoice => "invoices",
                DocumentType.Contract => "contracts",
                DocumentType.TaxDocument => "tax-documents",
                DocumentType.FinancialStatement => "financial-statements",
                _ => "documents"
            };
        }

        private string GenerateBlobPath(Guid? tenantId, string fileName)
        {
            var tenantPrefix = tenantId.HasValue ? $"{tenantId}/" : "host/";
            var datePrefix = $"{DateTime.UtcNow:yyyy/MM/dd}/";
            var uniqueId = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(fileName);
            
            return $"{tenantPrefix}{datePrefix}{uniqueId}{extension}";
        }
    }
}
