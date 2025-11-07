using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Approval.Data
{
    public interface IApprovalRequestRepository : IRepository<ApprovalRequest, Guid>
    {
        Task<ApprovalRequest> GetWithDecisionsAsync(Guid id);
        Task<List<ApprovalRequest>> GetPendingRequestsAsync();
        Task<List<ApprovalRequest>> GetRequestsByEntityAsync(string entityType, Guid entityId);
        Task<List<ApprovalRequest>> GetListAsync(ISpecification<ApprovalRequest> spec);
    }
}
