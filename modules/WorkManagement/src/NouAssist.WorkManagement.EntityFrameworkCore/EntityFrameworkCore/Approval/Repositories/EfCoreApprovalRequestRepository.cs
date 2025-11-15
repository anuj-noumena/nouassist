using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval.Repositories
{
    public class EfCoreApprovalRequestRepository 
        : EfCoreRepository<IApprovalDbContext, ApprovalRequest, Guid>, 
          IApprovalRequestRepository
    {
        public EfCoreApprovalRequestRepository(
            IDbContextProvider<IApprovalDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ApprovalRequest> GetWithDecisionsAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Include(r => r.Decisions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ApprovalRequest>> GetPendingRequestsAsync()
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.Status == ApprovalStatus.Pending || 
                           r.Status == ApprovalStatus.InProgress)
                .OrderBy(r => r.Priority)
                .ThenBy(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetRequestsByEntityAsync(
            string entityType, 
            Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.EntityType == entityType && r.EntityId == entityId)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetRequestsByStatusAsync(ApprovalStatus status)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetRequestsByWorkflowAsync(Guid workflowId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(r => r.WorkflowId == workflowId)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetOverdueRequestsAsync()
        {
            var dbSet = await GetDbSetAsync();
            var now = DateTime.UtcNow;
            
            return await dbSet
                .Where(r => r.DueDate.HasValue && 
                           r.DueDate.Value < now &&
                           r.Status != ApprovalStatus.Approved &&
                           r.Status != ApprovalStatus.Rejected &&
                           r.Status != ApprovalStatus.Cancelled)
                .OrderBy(r => r.DueDate)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetSlaBreachedRequestsAsync()
        {
            var dbSet = await GetDbSetAsync();
            var now = DateTime.UtcNow;
            
            return await dbSet
                .Where(r => r.SlaDeadline.HasValue && 
                           r.SlaDeadline.Value < now &&
                           r.Status != ApprovalStatus.Approved &&
                           r.Status != ApprovalStatus.Rejected &&
                           r.Status != ApprovalStatus.Cancelled)
                .OrderBy(r => r.SlaDeadline)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetRequestsByApproverAsync(Guid approverId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Include(r => r.Decisions)
                .Where(r => r.Decisions.Any(d => d.ApproverId == approverId))
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<ApprovalRequest>> GetListAsync(
            ISpecification<ApprovalRequest> spec)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync();
        }

        public override async Task<ApprovalRequest> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetWithDecisionsAsync(id);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }
    }
}
