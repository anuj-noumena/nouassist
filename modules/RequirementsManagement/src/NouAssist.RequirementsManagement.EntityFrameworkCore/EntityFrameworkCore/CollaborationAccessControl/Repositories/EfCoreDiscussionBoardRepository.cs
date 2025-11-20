using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Repositories
{
    public class EfCoreDiscussionBoardRepository : 
        CollaborationRepositoryBase<DiscussionBoard>, 
        IDiscussionBoardRepository
    {
        public EfCoreDiscussionBoardRepository(
            IDbContextProvider<ICollaborationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<DiscussionBoard>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.Threads)
                .ThenInclude(t => t.Comments);
        }

        public async Task<DiscussionBoard> GetByResourceAsync(
            TargetResourceType resourceType, 
            Guid resourceId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(d => 
                    EF.Property<string>(d, "Resource_Type") == resourceType.ToString() &&
                    EF.Property<Guid?>(d, "Resource_Id") == resourceId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DiscussionBoard>> GetByThreadCreatorAsync(
            Guid creatorId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(d => d.Threads.Any(t => t.CreatorId == creatorId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DiscussionBoard>> GetWithActiveThreadsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(d => d.IsActive && d.Threads.Any(t => !t.IsClosed))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DiscussionBoard>> GetByResourceTypeAsync(
            TargetResourceType resourceType, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(d => EF.Property<string>(d, "Resource_Type") == resourceType.ToString())
                .ToListAsync(cancellationToken);
        }
    }
}
