using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Projects.Data;
using NouAssist.Projects.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects.Repositories
{
    public class EfCoreProjectSuccessCriterionRepository 
        : EfCoreRepository<IProjectsDbContext, ProjectSuccessCriterion, Guid>, 
          IProjectSuccessCriterionRepository
    {
        public EfCoreProjectSuccessCriterionRepository(
            IDbContextProvider<IProjectsDbContext> dbContextProvider)
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
