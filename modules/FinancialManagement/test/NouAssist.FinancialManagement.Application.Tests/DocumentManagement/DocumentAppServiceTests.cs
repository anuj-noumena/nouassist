using System.Text;
using System.Threading.Tasks;
using NouAssist.DocumentManagement.Dtos;
using NouAssist.DocumentManagement.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.DocumentManagement
{
    public class DocumentAppServiceTests : DocumentManagementApplicationTestBase
    {
        private readonly IDocumentAppService _documentAppService;

        public DocumentAppServiceTests()
        {
            _documentAppService = GetRequiredService<IDocumentAppService>();
        }

        [Fact]
        public async Task Should_Upload_Document()
        {
            // Arrange
            var input = new CreateDocumentDto
            {
                FileName = "test.pdf",
                ContentType = "application/pdf",
                FileContent = Encoding.UTF8.GetBytes("Sample PDF content"),
                Type = DocumentType.Receipt,
                Description = "Test receipt"
            };

            // Act
            var result = await _documentAppService.UploadAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.FileName.ShouldBe("test.pdf");
            result.Type.ShouldBe(DocumentType.Receipt);
            result.Status.ShouldBe(DocumentStatus.Available);
        }

        [Fact]
        public async Task Should_Search_Documents()
        {
            // Arrange
            await UploadTestDocumentAsync();

            var input = new DocumentSearchDto
            {
                SearchTerm = "test",
                Type = DocumentType.Receipt,
                MaxResultCount = 10
            };

            // Act
            var result = await _documentAppService.SearchAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Should_Archive_Document()
        {
            // Arrange
            var document = await UploadTestDocumentAsync();

            // Act
            await _documentAppService.ArchiveAsync(document.Id);

            // Assert
            var archivedDoc = await _documentAppService.GetAsync(document.Id);
            archivedDoc.IsArchived.ShouldBeTrue();
            archivedDoc.ArchivedAt.ShouldNotBeNull();
        }

        private async Task<DocumentDto> UploadTestDocumentAsync()
        {
            return await _documentAppService.UploadAsync(new CreateDocumentDto
            {
                FileName = "test.pdf",
                ContentType = "application/pdf",
                FileContent = Encoding.UTF8.GetBytes("Test content"),
                Type = DocumentType.Receipt,
                Description = "Test document"
            });
        }
    }
}
