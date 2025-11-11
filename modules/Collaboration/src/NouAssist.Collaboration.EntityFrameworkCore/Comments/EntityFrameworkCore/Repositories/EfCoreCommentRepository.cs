using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Comments.Aggregates;
using NouAssist.Comments.Data;
using NouAssist.Comments.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Comments.EntityFrameworkCore.Repositories
{
    public class EfCoreCommentRepository 
        : EfCoreRepository<ICommentsDbContext, Comment, Guid>, ICommentRepository
    {
        public EfCoreCommentRepository(
            IDbContextProvider<ICommentsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Comment>> GetCommentsByEntityAsync(
            CommentableEntityType entityType,
            Guid entityId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.EntityType == entityType && 
                           c.EntityId == entityId && 
                           !c.IsDeleted)
                .OrderBy(c => c.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Comment>> GetRepliesAsync(
            Guid parentCommentId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(c => c.ParentCommentId == parentCommentId && !c.IsDeleted)
                .OrderBy(c => c.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Comment>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.Reactions);
        }
    }
}
