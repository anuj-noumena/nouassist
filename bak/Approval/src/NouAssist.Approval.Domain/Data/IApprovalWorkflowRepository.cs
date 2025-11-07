using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Approval.Data
{
    public interface IApprovalWorkflowRepository : IRepository<ApprovalWorkflow, Guid>
    {
        Task<ApprovalWorkflow> GetWithStepsAsync(Guid id);
        Task<List<ApprovalWorkflow>> GetActiveWorkflowsAsync();
        Task<List<ApprovalWorkflow>> GetByTypeAsync(ApprovalWorkflowType workflowType);
    }
}
