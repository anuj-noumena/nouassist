using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Approval.Data
{
    public interface IApprovalWorkflowRepository : IRepository<ApprovalWorkflow, Guid>
    {
        Task<ApprovalWorkflow> GetWithStepsAsync(Guid id);
        Task<List<ApprovalWorkflow>> GetActiveWorkflowsAsync();
        Task<List<ApprovalWorkflow>> GetByTypeAsync(ApprovalWorkflowType workflowType);
    }
}
