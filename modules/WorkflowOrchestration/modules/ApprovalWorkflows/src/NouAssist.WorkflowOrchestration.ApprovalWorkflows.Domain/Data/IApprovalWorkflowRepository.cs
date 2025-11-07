using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Data
{
    public interface IApprovalWorkflowRepository : IRepository<ApprovalWorkflow, Guid>
    {
        Task<ApprovalWorkflow> GetWithStepsAsync(Guid id);
        Task<List<ApprovalWorkflow>> GetActiveWorkflowsAsync();
        Task<List<ApprovalWorkflow>> GetByTypeAsync(ApprovalWorkflowType workflowType);
    }
}
