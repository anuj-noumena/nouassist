using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Repositories
{
    public class EfCoreProjectSuccessCriterionRepository 
        : EfCoreRepository<IWorkManagementDbContext, ProjectSuccessCriterion, Guid>, 
          IProjectSuccessCriterionRepository
    {
        public EfCoreProjectSuccessCriterionRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProjectSuccessCriterion>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(c => c.ProjectId == projectId)
                .OrderBy(c => c.OrderIndex)
                .ThenBy(c => c.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectSuccessCriterion>> GetUnmetCriteriaAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(c => c.ProjectId == projectId && !c.IsMet)
                .OrderBy(c => c.OrderIndex)
                .ToListAsync(cancellationToken);
        }
    }
}
