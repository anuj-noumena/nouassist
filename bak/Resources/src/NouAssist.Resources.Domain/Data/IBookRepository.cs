using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Resources.Data
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<List<Book>> GetListAsync(ISpecification<Book> spec, int maxResultCount = 50);
        Task<List<Book>> GetByReadingStatusAsync(Guid ownerId, ReadingStatus status);
        Task<List<Book>> GetByAuthorAsync(string authorName);
        Task<List<Book>> GetBySeriesAsync(string seriesName);
        Task<Book> FindByIsbnAsync(string isbn);
        Task<List<Book>> SearchAsync(string searchTerm, Guid? ownerId = null);
    }
}
