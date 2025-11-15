using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Learning.EntityFrameworkCore.Repositories
{
    public class EfCoreTopicRepository 
        : EfCoreRepository<ILearningDbContext, Topic, Guid>, ITopicRepository
    {
        public EfCoreTopicRepository(
            IDbContextProvider<ILearningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Topic> GetWithChildrenAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(t => t.Children)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Topic>> GetUserTopicsAsync(
            Guid userId,
            ISpecification<Topic> spec = null,
            CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync())
                .Where(t => t.UserId == userId);

            if (spec != null)
            {
                query = query.Where(spec.ToExpression());
            }

            return await query
                .OrderByDescending(t => t.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Topic>> GetRootTopicsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(t => t.UserId == userId && t.ParentId == null)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Topic>> GetChildrenAsync(
            Guid parentId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(t => t.ParentId == parentId)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Topic>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(t => t.Children);
        }
    }
}
