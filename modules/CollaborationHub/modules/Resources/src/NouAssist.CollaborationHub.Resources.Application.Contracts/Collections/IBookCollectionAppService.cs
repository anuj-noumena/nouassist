using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.Resources.Collections.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.Resources.Collections
{
    public interface IBookCollectionAppService : IApplicationService
    {
        Task<BookCollectionDto> GetAsync(Guid id);
        Task<List<BookCollectionDto>> GetMyCollectionsAsync();
        Task<BookCollectionDto> CreateAsync(CreateBookCollectionDto input);
        Task<BookCollectionDto> UpdateAsync(Guid id, UpdateBookCollectionDto input);
        Task DeleteAsync(Guid id);
        Task<BookCollectionDto> AddBookAsync(Guid collectionId, AddBookToCollectionDto input);
        Task RemoveBookAsync(Guid collectionId, Guid bookId);
        Task<BookCollectionDto> ReorderAsync(Guid id, int newSortOrder);
    }
}
