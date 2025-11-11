using System;
using System.Threading.Tasks;
using NouAssist.Resources.Books.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Resources.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<BookDto> GetAsync(Guid id);
        Task<PagedResultDto<BookDto>> GetListAsync(GetBookListDto input);
        Task<BookDto> CreateAsync(CreateBookDto input);
        Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input);
        Task DeleteAsync(Guid id);
        Task<BookDto> StartReadingAsync(Guid id);
        Task<BookDto> UpdateProgressAsync(Guid id, UpdateReadingProgressDto input);
        Task<BookDto> FinishReadingAsync(Guid id);
        Task<BookDto> MarkAsAbandonedAsync(Guid id);
        Task<BookDto> RateAsync(Guid id, RateBookDto input);
        Task<BookDto> AddToFavoritesAsync(Guid id);
        Task<BookDto> RemoveFromFavoritesAsync(Guid id);
        Task<BookDto> AddTagAsync(Guid id, string tag);
        Task<BookDto> RemoveTagAsync(Guid id, string tag);
        Task<PagedResultDto<BookDto>> SearchAsync(string searchTerm, PagedAndSortedResultRequestDto input);
    }
}
