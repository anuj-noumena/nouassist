using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Projects.Data;
using NouAssist.Projects.Enums;
using NouAssist.Workflow.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects.Repositories
{
    public class EfCoreProjectRequirementRepository 
        : EfCoreRepository<IProjectsDbContext, ProjectRequirement, Guid>, 
          IProjectRequirementRepository
    {
        public EfCoreProjectRequirementRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProjectRequirement>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(r => r.ProjectId == projectId)
                .OrderBy(r => r.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectRequirement>> GetPendingApprovalAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(r => r.Status == ProjectRequirementStatus.Proposed)
                .OrderBy(r => r.CreationTime)
                .ToListAsync(cancellationToken);
        }
    }
}
