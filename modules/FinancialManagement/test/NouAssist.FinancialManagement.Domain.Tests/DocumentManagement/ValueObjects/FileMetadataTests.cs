using NouAssist.DocumentManagement.ValueObjects;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.DocumentManagement.Tests.ValueObjects
{
    public class FileMetadataTests
    {
        [Fact]
        public void Constructor_WithValidData_Should_CreateMetadata()
        {
            // Act
            var metadata = new FileMetadata("test.pdf", "application/pdf", 1024);

            // Assert
            metadata.FileName.ShouldBe("test.pdf");
            metadata.ContentType.ShouldBe("application/pdf");
            metadata.FileSize.ShouldBe(1024);
            metadata.FileExtension.ShouldBe(".pdf");
            metadata.IsPDF.ShouldBeTrue();
        }

        [Fact]
        public void Constructor_WithInvalidFileSize_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<BusinessException>(() =>
                new FileMetadata("test.pdf", "application/pdf", 0));
        }

        [Fact]
        public void Constructor_WithUnsupportedContentType_Should_ThrowException()
        {
            // Act & Assert
            Should.Throw<BusinessException>(() =>
                new FileMetadata("test.exe", "application/x-msdownload", 1024));
        }

        [Fact]
        public void IsImage_Should_ReturnTrue_ForImageTypes()
        {
            // Arrange
            var metadata = new FileMetadata("photo.jpg", "image/jpeg", 2048);

            // Assert
            metadata.IsImage.ShouldBeTrue();
            metadata.IsPDF.ShouldBeFalse();
        }
    }
}
