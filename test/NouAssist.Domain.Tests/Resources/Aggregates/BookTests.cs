using System;
using NouAssist.Resources.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Resources.Aggregates
{
    public class BookTests
    {
        [Fact]
        public void StartReading_Should_UpdateStatus()
        {
            // Arrange
            var book = CreateTestBook();

            // Act
            book.StartReading();

            // Assert
            book.ReadingStatus.ShouldBe(ReadingStatus.Reading);
            book.StartedReadingAt.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateProgress_Should_CalculatePercentage()
        {
            // Arrange
            var book = CreateTestBook();

            // Act
            book.UpdateProgress(page: 50);

            // Assert
            book.CurrentPage.ShouldBe(50);
            book.ProgressPercentage.ShouldBeGreaterThan(0);
            book.ReadingStatus.ShouldBe(ReadingStatus.Reading);
        }

        [Fact]
        public void FinishReading_Should_SetProgress100Percent()
        {
            // Arrange
            var book = CreateTestBook();

            // Act
            book.FinishReading();

            // Assert
            book.ReadingStatus.ShouldBe(ReadingStatus.Finished);
            book.FinishedReadingAt.ShouldNotBeNull();
            book.ProgressPercentage.ShouldBe(100.0);
        }

        [Fact]
        public void Rate_Should_ThrowException_ForInvalidRating()
        {
            // Arrange
            var book = CreateTestBook();

            // Act & Assert
            Should.Throw<BusinessException>(() => book.Rate(6));
            Should.Throw<BusinessException>(() => book.Rate(0));
        }

        private Book CreateTestBook()
        {
            var book = new Book(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Book",
                null,
                format: BookFormat.Pdf
            );
            
            // Set page count for testing
            typeof(Book).GetProperty(nameof(Book.PageCount))
                .SetValue(book, 200);

            return book;
        }
    }
}
