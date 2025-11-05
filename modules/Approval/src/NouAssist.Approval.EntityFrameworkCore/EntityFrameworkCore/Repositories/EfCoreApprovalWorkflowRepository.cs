using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Data;
using NouAssist.Approval.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Approval.EntityFrameworkCore.Repositories
{
    public class EfCoreApprovalWorkflowRepository 
        : EfCoreRepository<IApprovalDbContext, ApprovalWorkflow, Guid>, 
          IApprovalWorkflowRepository
    {
        public EfCoreApprovalWorkflowRepository(
            IDbContextProvider<IApprovalDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ApprovalWorkflow> GetWithStepsAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<ApprovalWorkflow>> GetActiveWorkflowsAsync()
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(w => w.IsActive)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<List<ApprovalWorkflow>> GetByTypeAsync(ApprovalWorkflowType workflowType)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(w => w.WorkflowType == workflowType && w.IsActive)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public override async Task<ApprovalWorkflow> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetWithStepsAsync(id);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }
    }
}
