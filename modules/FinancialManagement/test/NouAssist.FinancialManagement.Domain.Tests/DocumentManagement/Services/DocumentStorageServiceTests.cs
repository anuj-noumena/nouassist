using System;
using System.Text;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Data;
using NouAssist.DocumentManagement.Enums;
using NouAssist.DocumentManagement.Services;
using NouAssist.DocumentManagement.ValueObjects;
using NSubstitute;
using Shouldly;
using Xunit;

namespace NouAssist.DocumentManagement.Tests.Services
{
    public class DocumentStorageServiceTests
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly DocumentStorageService _storageService;

        public DocumentStorageServiceTests()
        {
            _documentRepository = Substitute.For<IDocumentRepository>();
            _storageService = new DocumentStorageService(_documentRepository);
        }

        [Fact]
        public async Task CreateDocumentAsync_Should_CreateNewDocument()
        {
            // Arrange
            var fileName = "receipt.pdf";
            var contentType = "application/pdf";
            var fileContent = Encoding.UTF8.GetBytes("Sample PDF content");
            var fileSize = fileContent.Length;

            _documentRepository.FindByHashAsync(Arg.Any<string>()).Returns((Document)null);

            // Act
            var document = await _storageService.CreateDocumentAsync(
                null,
                fileName,
                contentType,
                fileSize,
                fileContent,
                DocumentType.Receipt,
                null,
                "Test receipt");

            // Assert
            document.ShouldNotBeNull();
            document.Metadata.FileName.ShouldBe(fileName);
            document.Metadata.ContentType.ShouldBe(contentType);
            document.Type.ShouldBe(DocumentType.Receipt);
            document.Status.ShouldBe(DocumentStatus.Available);
        }

        [Fact]
        public async Task CreateDocumentAsync_WithExistingHash_Should_ReturnExistingDocument()
        {
            // Arrange
            var fileName = "receipt.pdf";
            var contentType = "application/pdf";
            var fileContent = Encoding.UTF8.GetBytes("Sample PDF content");
            var fileSize = fileContent.Length;

            var existingDocument = CreateSampleDocument();
            _documentRepository.FindByHashAsync(Arg.Any<string>()).Returns(existingDocument);

            // Act
            var document = await _storageService.CreateDocumentAsync(
                null,
                fileName,
                contentType,
                fileSize,
                fileContent,
                DocumentType.Receipt);

            // Assert
            document.ShouldBe(existingDocument);
            await _documentRepository.DidNotReceive().InsertAsync(Arg.Any<Document>());
        }

        [Fact]
        public async Task DeleteDocumentAsync_WithReferences_Should_ThrowException()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var document = CreateSampleDocument();

            _documentRepository.GetAsync(documentId).Returns(document);
            _documentRepository.HasReferencesAsync(documentId).Returns(true);

            // Act & Assert
            await Should.ThrowAsync<BusinessException>(async () =>
                await _storageService.DeleteDocumentAsync(documentId, Guid.NewGuid()));
        }

        private Document CreateSampleDocument()
        {
            var metadata = new FileMetadata("test.pdf", "application/pdf", 1024);
            var storage = new StorageLocation("documents", "test/test.pdf");
            
            return new Document(
                Guid.NewGuid(),
                null,
                metadata,
                storage,
                DocumentType.Receipt);
        }
    }
}
