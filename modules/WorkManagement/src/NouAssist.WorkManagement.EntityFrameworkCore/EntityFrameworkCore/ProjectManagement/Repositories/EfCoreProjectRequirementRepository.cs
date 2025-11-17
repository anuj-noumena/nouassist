using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Repositories
{
    public class EfCoreProjectRequirementRepository 
        : EfCoreRepository<IWorkManagementDbContext, ProjectRequirement, Guid>, 
          IProjectRequirementRepository
    {
        public EfCoreProjectRequirementRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
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
