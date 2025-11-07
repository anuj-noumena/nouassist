using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Projects.Data;
using NouAssist.Projects.Entities;
using NouAssist.Projects.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects.Repositories
{
    public class EfCoreProjectDependencyRepository 
        : EfCoreRepository<IProjectsDbContext, ProjectDependency, Guid>, IProjectDependencyRepository
    {
        public EfCoreProjectDependencyRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProjectDependency>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(d => d.ProjectId == projectId)
                .OrderBy(d => d.IsBlocking ? 0 : 1)
                .ThenBy(d => d.ExpectedResolutionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectDependency>> GetBlockingDependenciesAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(d => d.ProjectId == projectId &&
                           d.IsBlocking &&
                           d.Status == ProjectDependencyStatus.Active &&
                           !d.IsResolved)
                .OrderBy(d => d.ExpectedResolutionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectDependency>> GetOverdueDependenciesAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.UtcNow;

            return await query
                .Where(d => d.ExpectedResolutionDate.HasValue &&
                           d.ExpectedResolutionDate.Value < now &&
                           d.Status == ProjectDependencyStatus.Active)
                .OrderBy(d => d.ExpectedResolutionDate)
                .ToListAsync(cancellationToken);
        }
    }
}
