using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreStrategicPivotRepository 
        : EfCoreRepository<IBusinessModelManagementDbContext, StrategicPivot, Guid>, 
          IStrategicPivotRepository
    {
        public EfCoreStrategicPivotRepository(
            IDbContextProvider<IBusinessModelManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<StrategicPivot>> GetByTypeAsync(
            PivotType type, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.Type == type && sp.TenantId == tenantId)
                .OrderByDescending(sp => sp.InitiatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetByStatusAsync(
            PivotStatus status, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.Status == status && sp.TenantId == tenantId)
                .OrderByDescending(sp => sp.InitiatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetByInitiatorAsync(
            Guid initiatorId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.InitiatedBy == initiatorId)
                .OrderByDescending(sp => sp.InitiatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetActivePivotsAsync(
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.TenantId == tenantId && 
                           (sp.Status == PivotStatus.InProgress || sp.Status == PivotStatus.Planned))
                .OrderByDescending(sp => sp.InitiatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetOverduePivotsAsync(
            DateTime currentDate, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.TenantId == tenantId && 
                           sp.Status != PivotStatus.Completed && 
                           sp.Status != PivotStatus.Abandoned && 
                           sp.TargetCompletionDate < currentDate)
                .OrderBy(sp => sp.TargetCompletionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<StrategicPivot> GetWithImpactMetricsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(sp => sp.ImpactMetrics)
                .FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(sp => sp.TenantId == tenantId && 
                           sp.InitiatedDate >= startDate && 
                           sp.InitiatedDate <= endDate)
                .OrderByDescending(sp => sp.InitiatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<StrategicPivot> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<StrategicPivot> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetWithImpactMetricsAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }

        public async Task<List<StrategicPivot>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting = null,
            PivotType? type = null,
            PivotStatus? status = null,
            Guid? initiatorId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (type.HasValue)
            {
                query = query.Where(sp => sp.Type == type.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(sp => sp.Status == status.Value);
            }

            if (initiatorId.HasValue)
            {
                query = query.Where(sp => sp.InitiatedBy == initiatorId.Value);
            }

            query = query
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(StrategicPivot.InitiatedDate) + " desc" : sorting)
                .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
