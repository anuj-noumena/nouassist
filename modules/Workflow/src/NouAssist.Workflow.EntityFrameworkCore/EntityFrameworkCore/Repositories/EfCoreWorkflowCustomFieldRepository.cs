using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Workflow.Data;
using NouAssist.Workflow.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Workflow.EntityFrameworkCore.Repositories
{
    public class EfCoreWorkflowCustomFieldRepository
        : EfCoreRepository<IWorkflowDbContext, WorkflowCustomField, Guid>, IWorkflowCustomFieldRepository
    {
        public EfCoreWorkflowCustomFieldRepository(
            IDbContextProvider<IWorkflowDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<WorkflowCustomField>> GetByWorkflowSchemeIdAsync(
            Guid workflowSchemeId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(wcf => wcf.WorkflowSchemeId == workflowSchemeId && wcf.IsActive)
                .OrderBy(wcf => wcf.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<WorkflowCustomField>> GetVisibleInStatusAsync(
            Guid workflowSchemeId,
            Guid statusId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var allFields = await dbSet
                .Where(wcf => wcf.WorkflowSchemeId == workflowSchemeId && wcf.IsActive)
                .OrderBy(wcf => wcf.DisplayOrder)
                .ToListAsync(cancellationToken);

            // Filter in memory for status visibility
            return allFields
                .Where(wcf => wcf.IsVisibleInStatus(statusId))
                .ToList();
        }
    }
}
