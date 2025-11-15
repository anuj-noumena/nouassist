using System;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Data;
using NouAssist.DocumentManagement.Services;
using NSubstitute;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.DocumentManagement.Tests.Services
{
    public class DocumentFolderServiceTests
    {
        private readonly IDocumentFolderRepository _folderRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly DocumentFolderService _folderService;

        public DocumentFolderServiceTests()
        {
            _folderRepository = Substitute.For<IDocumentFolderRepository>();
            _documentRepository = Substitute.For<IDocumentRepository>();
            _folderService = new DocumentFolderService(_folderRepository, _documentRepository);
        }

        [Fact]
        public async Task CreateFolderAsync_Should_CreateRootFolder()
        {
            // Arrange
            var folderName = "Invoices";

            // Act
            var folder = await _folderService.CreateFolderAsync(null, folderName);

            // Assert
            folder.ShouldNotBeNull();
            folder.Name.ShouldBe(folderName);
            folder.Path.ShouldBe(folderName);
            folder.Level.ShouldBe(0);
            folder.ParentFolderId.ShouldBeNull();
        }

        [Fact]
        public async Task CreateFolderAsync_WithParent_Should_CreateChildFolder()
        {
            // Arrange
            var parentId = Guid.NewGuid();
            var parentFolder = new DocumentFolder(parentId, null, "Parent");
            
            _folderRepository.GetAsync(parentId).Returns(parentFolder);

            // Act
            var folder = await _folderService.CreateFolderAsync(null, "Child", parentId);

            // Assert
            folder.Name.ShouldBe("Child");
            folder.Path.ShouldBe("Parent/Child");
            folder.Level.ShouldBe(1);
            folder.ParentFolderId.ShouldBe(parentId);
        }

        [Fact]
        public async Task MoveFolderAsync_ToSelf_Should_ThrowException()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var folder = new DocumentFolder(folderId, null, "Test");

            _folderRepository.GetAsync(folderId).Returns(folder);

            // Act & Assert
            await Should.ThrowAsync<BusinessException>(async () =>
                await _folderService.MoveFolderAsync(folderId, folderId));
        }

        [Fact]
        public async Task AddDocumentToFolderAsync_Should_AddDocument()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var documentId = Guid.NewGuid();
            var folder = new DocumentFolder(folderId, null, "Test");
            var document = CreateSampleDocument();

            _folderRepository.GetAsync(folderId).Returns(folder);
            _documentRepository.GetAsync(documentId).Returns(document);

            // Act
            await _folderService.AddDocumentToFolderAsync(folderId, documentId);

            // Assert
            folder.DocumentIds.ShouldContain(documentId);
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
