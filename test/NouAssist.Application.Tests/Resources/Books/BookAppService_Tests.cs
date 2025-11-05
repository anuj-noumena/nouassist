using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Books.Dtos;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.Resources.Books
{
    public class BookAppService_Tests : ResourcesApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;
        private readonly IBookRepository _bookRepository;
        private readonly IResourceRepository _resourceRepository;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>();
            _bookRepository = GetRequiredService<IBookRepository>();
            _resourceRepository = GetRequiredService<IResourceRepository>();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_A_Valid_Book()
        {
            // Arrange
            var resource = await CreateTestResourceAsync();
            var input = new CreateBookDto
            {
                ResourceId = resource.Id,
                Title = "Clean Architecture",
                Authors = new List<string> { "Robert C. Martin" },
                Publisher = "Prentice Hall",
                Isbn13 = "9780134494166",
                Format = BookFormat.Physical
            };

            // Act
            var result = await _bookAppService.CreateAsync(input);

            // Assert
            result.Id.ShouldNotBe(Guid.Empty);
            result.Title.ShouldBe(input.Title);
            result.Authors.Count.ShouldBe(1);
            
            var bookInDb = await _bookRepository.FindAsync(result.Id);
            bookInDb.ShouldNotBeNull();
            bookInDb.ReadingStatus.ShouldBe(ReadingStatus.ToRead);
        }

        [Fact]
        public async Task StartReadingAsync_Should_Update_Status()
        {
            // Arrange
            var book = await CreateTestBookAsync();

            // Act
            var result = await _bookAppService.StartReadingAsync(book.Id);

            // Assert
            result.ReadingStatus.ShouldBe(ReadingStatus.Reading.ToString());
            result.StartedReadingAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateProgressAsync_Should_Track_Reading()
        {
            // Arrange
            var book = await CreateTestBookAsync();
            await _bookAppService.StartReadingAsync(book.Id);

            // Act
            var result = await _bookAppService.UpdateProgressAsync(
                book.Id,
                new UpdateReadingProgressDto { CurrentPage = 150 }
            );

            // Assert
            result.CurrentPage.ShouldBe(150);
            result.ProgressPercentage.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task RateAsync_Should_Add_Rating()
        {
            // Arrange
            var book = await CreateTestBookAsync();

            // Act
            var result = await _bookAppService.RateAsync(
                book.Id,
                new RateBookDto { Rating = 5, Review = "Excellent book!" }
            );

            // Assert
            result.UserRating.ShouldBe(5);
            result.UserReview.ShouldBe("Excellent book!");
        }

        [Fact]
        public async Task SearchAsync_Should_Find_Books()
        {
            // Arrange
            await CreateTestBookAsync("The Pragmatic Programmer");
            await CreateTestBookAsync("Clean Code");

            // Act
            var results = await _bookAppService.SearchAsync(
                "Pragmatic",
                new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto()
            );

            // Assert
            results.TotalCount.ShouldBeGreaterThan(0);
            results.Items.ShouldContain(b => b.Title.Contains("Pragmatic"));
        }

        private async Task<Resource> CreateTestResourceAsync()
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var resource = new Resource(
                    GuidGenerator.Create(),
                    CurrentUser.Id.Value,
                    ResourceKind.Document,
                    null
                );
                return await _resourceRepository.InsertAsync(resource);
            });
        }

        private async Task<Book> CreateTestBookAsync(string title = "Test Book")
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var resource = await CreateTestResourceAsync();
                var book = new Book(
                    GuidGenerator.Create(),
                    CurrentUser.Id.Value,
                    resource.Id,
                    title,
                    null,
                    new List<string> { "Test Author" },
                    BookFormat.Pdf
                );
                return await _bookRepository.InsertAsync(book);
            });
        }
    }
}
