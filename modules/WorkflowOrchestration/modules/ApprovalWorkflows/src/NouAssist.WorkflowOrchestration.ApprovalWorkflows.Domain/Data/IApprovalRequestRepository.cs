using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Data
{
    public interface IApprovalRequestRepository : IRepository<ApprovalRequest, Guid>
    {
        Task<ApprovalRequest> GetWithDecisionsAsync(Guid id);
        Task<List<ApprovalRequest>> GetPendingRequestsAsync();
        Task<List<ApprovalRequest>> GetRequestsByEntityAsync(string entityType, Guid entityId);
        Task<List<ApprovalRequest>> GetListAsync(ISpecification<ApprovalRequest> spec);
    }
}
