using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Repositories
{
    public class EfCorePrioritizationActivityRepository 
        : EfCoreRepository<IRequirementsAnalysisToolsDbContext, PrioritizationActivity, Guid>, 
          IPrioritizationActivityRepository
    {
        public EfCorePrioritizationActivityRepository(
            IDbContextProvider<IRequirementsAnalysisToolsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<PrioritizationActivity>> GetListAsync(
            Specification<PrioritizationActivity> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .Include(pa => pa.Votes)
                .OrderByDescending(pa => pa.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetByStatusAsync(
            ActivityStatus status,
            CancellationToken cancellationToken = default)
        {
            var specification = new ActivityStatusSpecification(status);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetByParticipantAsync(
            Guid stakeholderId,
            CancellationToken cancellationToken = default)
        {
            var specification = new StakeholderParticipatingSpecification(stakeholderId);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetByRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var specification = new ContainsRequirementSpecification(requirementId);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetByMethodAsync(
            PrioritizationMethod method,
            CancellationToken cancellationToken = default)
        {
            var specification = new PrioritizationMethodSpecification(method);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetActiveActivitiesAsync(
            CancellationToken cancellationToken = default)
        {
            var specification = new ActiveActivitiesSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetCompletedActivitiesAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(pa => pa.Status == ActivityStatus.Completed &&
                            pa.EndDate.HasValue &&
                            pa.EndDate >= fromDate &&
                            pa.EndDate <= toDate)
                .Include(pa => pa.Votes)
                .OrderByDescending(pa => pa.EndDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PrioritizationActivity>> GetOverdueActivitiesAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default)
        {
            var specification = new OverdueActivitiesSpecification(currentDate);
            return await GetListAsync(specification, cancellationToken);
        }

        public override async Task<IQueryable<PrioritizationActivity>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(pa => pa.Votes);
        }

        public override async Task<PrioritizationActivity> GetAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(pa => pa.Votes);
            }

            var activity = await query
                .FirstOrDefaultAsync(pa => pa.Id == id, cancellationToken);

            if (activity == null)
            {
                throw new EntityNotFoundException(typeof(PrioritizationActivity), id);
            }

            return activity;
        }

        public override async Task<PrioritizationActivity?> FindAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(pa => pa.Votes);
            }

            return await query
                .FirstOrDefaultAsync(pa => pa.Id == id, cancellationToken);
        }

        public override async Task<List<PrioritizationActivity>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(pa => pa.Votes);
            }

            return await query
                .OrderByDescending(pa => pa.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public override async Task<List<PrioritizationActivity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(pa => pa.Votes);
            }

            return await query
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(PrioritizationActivity.CreationTime) + " DESC" : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
