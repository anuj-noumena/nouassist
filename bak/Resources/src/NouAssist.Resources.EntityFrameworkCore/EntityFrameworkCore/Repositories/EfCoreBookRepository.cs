using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Resources.EntityFrameworkCore.Repositories
{
    public class EfCoreBookRepository 
        : EfCoreRepository<IResourcesDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(
            IDbContextProvider<IResourcesDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Book>> GetListAsync(
            ISpecification<Book> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(b => b.CreationTime)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByReadingStatusAsync(Guid ownerId, ReadingStatus status)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(b => b.OwnerId == ownerId && b.ReadingStatus == status && !b.IsDeleted)
                .OrderByDescending(b => b.LastModificationTime ?? b.CreationTime)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByAuthorAsync(string authorName)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(b => b.Authors.Contains(authorName) && !b.IsDeleted)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<List<Book>> GetBySeriesAsync(string seriesName)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(b => b.SeriesName == seriesName && !b.IsDeleted)
                .OrderBy(b => b.SeriesPosition ?? 0)
                .ThenBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book> FindByIsbnAsync(string isbn)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(b => (b.Isbn13 == isbn || b.Isbn10 == isbn) && !b.IsDeleted);
        }

        public async Task<List<Book>> SearchAsync(string searchTerm, Guid? ownerId = null)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(b => !b.IsDeleted);

            if (ownerId.HasValue)
            {
                query = query.Where(b => b.OwnerId == ownerId.Value);
            }

            // PostgreSQL full-text search
            return await query
                .Where(b => EF.Functions.Like(b.Title, $"%{searchTerm}%") ||
                           b.Authors.Contains(searchTerm) ||
                           (b.Description != null && EF.Functions.Like(b.Description, $"%{searchTerm}%")))
                .OrderBy(b => b.Title)
                .Take(50)
                .ToListAsync();
        }
    }
}
