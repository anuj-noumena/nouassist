using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Repositories
{
    public class EfCoreRequirementReviewCycleRepository 
        : EfCoreRepository<IValidationVerificationDbContext, RequirementReviewCycle, Guid>, IRequirementReviewCycleRepository 
    {
        public EfCoreRequirementReviewCycleRepository(
            IDbContextProvider<IValidationVerificationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<RequirementReviewCycle>> GetActiveReviewCyclesForRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rc => rc.RequirementId == requirementId && 
                            (rc.Status == ReviewCycleStatus.Draft || rc.Status == ReviewCycleStatus.InProgress))
                .Include(rc => rc.ReviewerAssignments)
                .Include(rc => rc.WorkflowInstance)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementReviewCycle>> GetReviewCyclesByStatusAsync(
            ReviewCycleStatus status,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(rc => rc.TenantId == tenantId);
            }

            return await query
                .Where(rc => rc.Status == status)
                .Include(rc => rc.ReviewerAssignments)
                .Include(rc => rc.WorkflowInstance)
                .OrderByDescending(rc => rc.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementReviewCycle>> GetOverdueReviewCyclesAsync(
            DateTime currentTime,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(rc => rc.TenantId == tenantId);
            }

            return await query
                .Where(rc => rc.Status == ReviewCycleStatus.InProgress &&
                            rc.TargetCompletionDate.HasValue &&
                            rc.TargetCompletionDate.Value < currentTime)
                .Include(rc => rc.ReviewerAssignments)
                .Include(rc => rc.FeedbackItems)
                .OrderBy(rc => rc.TargetCompletionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementReviewCycle>> GetReviewCyclesForReviewerAsync(
            Guid reviewerId,
            ReviewCycleStatus? status = null,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(rc => rc.TenantId == tenantId);
            }

            query = query.Where(rc => rc.ReviewerAssignments.Any(ra => ra.ReviewerId == reviewerId));

            if (status.HasValue)
            {
                query = query.Where(rc => rc.Status == status.Value);
            }

            return await query
                .Include(rc => rc.ReviewerAssignments)
                .Include(rc => rc.FeedbackItems)
                .OrderByDescending(rc => rc.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementReviewCycle>> GetReviewCyclesInDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(rc => rc.TenantId == tenantId);
            }

            return await query
                .Where(rc => rc.CreationTime >= startDate && rc.CreationTime <= endDate)
                .Include(rc => rc.ReviewerAssignments)
                .Include(rc => rc.FeedbackItems)
                .Include(rc => rc.WorkflowInstance)
                .OrderBy(rc => rc.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementReviewSummary>> GetRequirementReviewSummariesAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            var summaries = await query
                .Where(rc => requirementIds.Contains(rc.RequirementId))
                .GroupBy(rc => rc.RequirementId)
                .Select(g => new RequirementReviewSummary
                {
                    RequirementId = g.Key,
                    TotalReviewCycles = g.Count(),
                    CompletedReviewCycles = g.Count(rc => rc.Status == ReviewCycleStatus.Completed),
                    InProgressReviewCycles = g.Count(rc => rc.Status == ReviewCycleStatus.InProgress),
                    LastReviewDate = g.Max(rc => rc.ActualCompletionDate ?? rc.CreationTime),
                    LastReviewStatus = g.OrderByDescending(rc => rc.CreationTime).FirstOrDefault().Status,
                    AverageReviewDuration = g.Where(rc => rc.ActualCompletionDate.HasValue)
                        .Average(rc => EF.Functions.DateDiffDay(rc.CreationTime, rc.ActualCompletionDate.Value))
                })
                .ToListAsync(cancellationToken);

            return summaries;
        }

        public async Task<Dictionary<ReviewCycleStatus, int>> GetReviewCycleCountsByStatusAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(rc => rc.TenantId == tenantId);
            }

            var counts = await query
                .GroupBy(rc => rc.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            return counts.ToDictionary(x => x.Status, x => x.Count);
        }

        // Override to include details when needed
        public override async Task<RequirementReviewCycle> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(rc => rc.ReviewerAssignments)
                    .Include(rc => rc.FeedbackItems)
                    .Include(rc => rc.WorkflowInstance);
            }

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(RequirementReviewCycle), id);
            }

            return entity;
        }

        public override async Task<RequirementReviewCycle> FindAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(rc => rc.ReviewerAssignments)
                    .Include(rc => rc.FeedbackItems)
                    .Include(rc => rc.WorkflowInstance);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
