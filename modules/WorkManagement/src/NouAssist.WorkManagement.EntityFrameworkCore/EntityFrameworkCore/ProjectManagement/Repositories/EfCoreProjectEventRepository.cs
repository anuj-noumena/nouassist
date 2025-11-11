using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.ProjectManagement.Data;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects.Repositories
{
    public class EfCoreProjectEventRepository 
        : EfCoreRepository<IProjectsDbContext, ProjectEvent, Guid>, IProjectEventRepository
    {
        public EfCoreProjectEventRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProjectEvent>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(e => e.ProjectId == projectId)
                .OrderBy(e => e.EventDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectEvent>> GetUpcomingEventsAsync(
            Guid projectId,
            DateTime fromDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(e => e.ProjectId == projectId &&
                           e.EventDate >= fromDate &&
                           !e.IsCompleted)
                .OrderBy(e => e.EventDate)
                .ToListAsync(cancellationToken);
        }
    }
}
