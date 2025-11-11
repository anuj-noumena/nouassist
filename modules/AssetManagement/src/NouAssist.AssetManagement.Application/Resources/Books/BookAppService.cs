using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Books.Dtos;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using NouAssist.Resources.Permissions;
using NouAssist.Resources.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Resources.Books
{
    [Authorize(ResourcesPermissions.Books.Default)]
    public class BookAppService : ApplicationService, IBookAppService
    {
        private readonly IBookRepository _bookRepository;

        public BookAppService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            
            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to access this book.");
            }

            return MapToDto(book);
        }

        public async Task<PagedResultDto<BookDto>> GetListAsync(GetBookListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var books = await _bookRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _bookRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<BookDto>(
                totalCount,
                books.Select(MapToDto).ToList()
            );
        }

        [Authorize(ResourcesPermissions.Books.Create)]
        public async Task<BookDto> CreateAsync(CreateBookDto input)
        {
            var book = new Book(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.ResourceId,
                input.Title,
                CurrentTenant.Id,
                input.Authors,
                input.Format
            );

            if (!string.IsNullOrWhiteSpace(input.Subtitle))
            {
                book.UpdateMetadata(subtitle: input.Subtitle);
            }

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                book.UpdateMetadata(description: input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Publisher))
            {
                book.UpdateMetadata(publisher: input.Publisher);
            }

            if (input.Categories != null && input.Categories.Count > 0)
            {
                book.UpdateMetadata(categories: input.Categories);
            }

            if (input.Tags != null && input.Tags.Count > 0)
            {
                book.UpdateMetadata(tags: input.Tags);
            }

            await _bookRepository.InsertAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Update)]
        public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.UpdateMetadata(
                input.Title,
                input.Authors,
                input.Description,
                input.Publisher,
                input.Categories,
                input.Tags
            );

            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to delete this book.");
            }

            book.Delete(CurrentUser.Id.Value.ToString());
            await _bookRepository.UpdateAsync(book);
        }

        [Authorize(ResourcesPermissions.Books.UpdateReadingProgress)]
        public async Task<BookDto> StartReadingAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.StartReading();
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.UpdateReadingProgress)]
        public async Task<BookDto> UpdateProgressAsync(Guid id, UpdateReadingProgressDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.UpdateProgress(input.CurrentPage, input.ProgressPercentage);
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.UpdateReadingProgress)]
        public async Task<BookDto> FinishReadingAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.FinishReading();
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.UpdateReadingProgress)]
        public async Task<BookDto> MarkAsAbandonedAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.MarkAsAbandoned();
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Rate)]
        public async Task<BookDto> RateAsync(Guid id, RateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to rate this book.");
            }

            book.Rate(input.Rating);

            if (!string.IsNullOrWhiteSpace(input.Review))
            {
                book.SetReview(input.Review);
            }

            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Update)]
        public async Task<BookDto> AddToFavoritesAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.AddToFavorites();
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Update)]
        public async Task<BookDto> RemoveFromFavoritesAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.RemoveFromFavorites();
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Update)]
        public async Task<BookDto> AddTagAsync(Guid id, string tag)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.AddTag(tag);
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        [Authorize(ResourcesPermissions.Books.Update)]
        public async Task<BookDto> RemoveTagAsync(Guid id, string tag)
        {
            var book = await _bookRepository.GetAsync(id);

            // Authorization check
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Books:NotAuthorized", "You are not authorized to update this book.");
            }

            book.RemoveTag(tag);
            await _bookRepository.UpdateAsync(book);

            return MapToDto(book);
        }

        public async Task<PagedResultDto<BookDto>> SearchAsync(string searchTerm, PagedAndSortedResultRequestDto input)
        {
            var books = await _bookRepository.SearchAsync(searchTerm, CurrentUser.Id.Value);
            
            var pagedBooks = books
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<BookDto>(
                books.Count,
                pagedBooks.Select(MapToDto).ToList()
            );
        }

        private ISpecification<Book> CreateListSpecification(GetBookListDto input)
        {
            var spec = new Specification<Book>(b => b.OwnerId == CurrentUser.Id.Value);

            if (input.ReadingStatus.HasValue)
            {
                spec = spec.And(new BooksByReadingStatusSpecification(input.ReadingStatus.Value));
            }

            if (input.IsFavorite.HasValue && input.IsFavorite.Value)
            {
                spec = spec.And(new FavoriteBooksSpecification(CurrentUser.Id.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Author))
            {
                spec = spec.And(new BooksByAuthorSpecification(input.Author));
            }

            if (!string.IsNullOrWhiteSpace(input.SeriesName))
            {
                spec = spec.And(new BooksInSeriesSpecification(input.SeriesName));
            }

            if (!string.IsNullOrWhiteSpace(input.CollectionName))
            {
                spec = spec.And(new Specification<Book>(b => b.CollectionName == input.CollectionName));
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                spec = spec.And(new Specification<Book>(b => 
                    b.Title.Contains(input.SearchTerm) || 
                    b.Authors.Any(a => a.Contains(input.SearchTerm))));
            }

            return spec;
        }

        private BookDto MapToDto(Book book)
        {
            var dto = ObjectMapper.Map<Book, BookDto>(book);
            dto.DisplayAuthors = book.GetDisplayAuthors();
            dto.PrimaryIsbn = book.GetPrimaryIsbn();
            dto.ReadingProgressDisplay = book.GetReadingProgressDisplay();
            return dto;
        }
    }
}
