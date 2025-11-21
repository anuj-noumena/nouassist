using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using NouAssist.RequirementsManagement.ChangeManagement.Repositories;
using NouAssist.RequirementsManagement.ChangeManagement.Specifications;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IRequirementChangeRequestRepository
    /// </summary>
    public class EfCoreRequirementChangeRequestRepository 
        : EfCoreRepository<ChangeManagementDbContext, RequirementChangeRequest, Guid>,
          IRequirementChangeRequestRepository
    {
        public EfCoreRequirementChangeRequestRepository(
            IDbContextProvider<ChangeManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<RequirementChangeRequest>> GetListAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<RequirementChangeRequest?> FindByRequestIdentifierAsync(
            RequestIdentifier requestIdentifier,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(x => 
                    EF.Property<string>(x, "RequestIdentifier_Value") == requestIdentifier.Value, 
                    cancellationToken);
        }

        public async Task<List<RequirementChangeRequest>> GetByAffectedRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(x => EF.Property<Guid>(x, "AffectedRequirementId") == requirementId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementChangeRequest>> GetBySubmitterAsync(
            Guid submitterId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(x => x.SubmitterId == submitterId)
                .OrderByDescending(x => x.SubmissionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementChangeRequest>> GetPendingForApprovalByUserAsync(
            Guid approverId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // Include approval steps for checking
            query = query.Include(x => x.ApprovalSteps);

            return await query
                .Where(x => x.Status == RequestStatus.PendingApproval)
                .Where(x => x.ApprovalSteps.Any(a => 
                    a.ApproverId == approverId && 
                    a.Decision == ApprovalDecision.Pending))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementChangeRequest>> GetHighImpactChangesAsync(
            decimal costThreshold = 10000m,
            int timeThreshold = 30,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // Include impact statements
            query = query.Include(x => x.ImpactStatements);

            return await query
                .Where(x => x.ImpactStatements.Any(i => 
                    (EF.Property<decimal?>(i, "CostImpact") != null && 
                     EF.Property<decimal?>(i, "CostImpact") > costThreshold) ||
                    (EF.Property<int?>(i, "TimeImpactDays") != null && 
                     EF.Property<int?>(i, "TimeImpactDays") > timeThreshold)))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RequirementChangeRequest>> GetOverdueChangesAsync(
            int overdueDays = 30,
            CancellationToken cancellationToken = default)
        {
            var overdueDate = DateTime.UtcNow.AddDays(-overdueDays);
            var query = await GetQueryableAsync();

            return await query
                .Where(x => x.SubmissionDate < overdueDate)
                .Where(x => x.Status != RequestStatus.Completed && x.Status != RequestStatus.Cancelled)
                .OrderBy(x => x.SubmissionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsRequestIdentifierUniqueAsync(
            RequestIdentifier requestIdentifier,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return !await query
                .AnyAsync(x => 
                    EF.Property<string>(x, "RequestIdentifier_Value") == requestIdentifier.Value, 
                    cancellationToken);
        }

        public override async Task<IQueryable<RequirementChangeRequest>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.Rationales)
                .Include(x => x.ImpactStatements)
                .Include(x => x.ApprovalSteps);
        }
    }
}
