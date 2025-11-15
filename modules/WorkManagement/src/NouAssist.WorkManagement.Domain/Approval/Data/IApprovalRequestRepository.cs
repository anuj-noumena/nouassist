using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Data
{
    public interface IApprovalRequestRepository : IRepository<ApprovalRequest, Guid>
    {
        Task<ApprovalRequest> GetWithDecisionsAsync(Guid id);
        Task<List<ApprovalRequest>> GetPendingRequestsAsync();
        Task<List<ApprovalRequest>> GetRequestsByEntityAsync(string entityType, Guid entityId);
        Task<List<ApprovalRequest>> GetListAsync(ISpecification<ApprovalRequest> spec);
        
        // New methods for enhanced querying
        Task<List<ApprovalRequest>> GetRequestsByStatusAsync(ApprovalStatus status);
        Task<List<ApprovalRequest>> GetRequestsByWorkflowAsync(Guid workflowId);
        Task<List<ApprovalRequest>> GetOverdueRequestsAsync();
        Task<List<ApprovalRequest>> GetSlaBreachedRequestsAsync();
        Task<List<ApprovalRequest>> GetRequestsByApproverAsync(Guid approverId);
    }
}