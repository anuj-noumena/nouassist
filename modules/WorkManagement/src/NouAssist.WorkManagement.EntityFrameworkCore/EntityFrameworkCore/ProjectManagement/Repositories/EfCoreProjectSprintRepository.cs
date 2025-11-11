using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Repositories
{
    public class EfCoreProjectSprintRepository 
        : EfCoreRepository<IProjectsDbContext, ProjectSprint, Guid>, IProjectSprintRepository
    {
        public EfCoreProjectSprintRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProjectSprint>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(s => s.ProjectId == projectId)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProjectSprint> GetActiveSprintAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.UtcNow;

            return await query
                .Where(s => s.ProjectId == projectId &&
                           s.Status == SprintStatus.InProgress &&
                           s.StartDate <= now &&
                           s.EndDate >= now)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
