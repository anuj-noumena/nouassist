using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Data;
using NouAssist.Projects.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.EntityFrameworkCore.Projects.Repositories
{
    public class EfCoreProjectRepository 
        : EfCoreRepository<IProjectsDbContext, Project, Guid>, IProjectRepository
    {
        public EfCoreProjectRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Project> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Project>> GetListAsync(
            ISpecification<Project> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.Where(specification.ToExpression());

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => p.OwnerId == ownerId && !p.IsArchived)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetOverdueProjectsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.UtcNow;

            return await query
                .Where(p => p.DueDate.HasValue &&
                           p.DueDate.Value < now &&
                           p.Status != ProjectStatus.Completed &&
                           p.Status != ProjectStatus.Cancelled &&
                           !p.IsArchived)
                .OrderBy(p => p.DueDate)
                .ToListAsync(cancellationToken);
        }
    }
}
