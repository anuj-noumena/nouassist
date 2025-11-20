using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories
{
    public class EfCoreTrackedMetricRepository 
        : EfCoreRepository<IPerformanceFinancialModelingDbContext, TrackedMetric, Guid>, ITrackedMetricRepository
    {
        public EfCoreTrackedMetricRepository(
            IDbContextProvider<IPerformanceFinancialModelingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TrackedMetric> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(m => m.Name == name && m.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TrackedMetric>> GetByTypeAsync(MetricType type, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(m => m.Type == type && m.TenantId == tenantId && m.IsActive)
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TrackedMetric>> GetActiveMetricsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(m => m.IsActive && m.TenantId == tenantId)
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TrackedMetric>> GetMetricsDueForUpdateAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var specification = new MetricsDueForUpdateSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<TrackedMetric>> GetMetricsWithActiveGoalsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var specification = new MetricsWithActiveGoalsSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<TrackedMetric>> GetListAsync(TrackedMetricSpecification specification, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(TrackedMetricSpecification specification, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        // Override to ensure proper includes for aggregate
        public override async Task<TrackedMetric> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                // Include child entities
                query = query
                    .Include(m => m.DataPoints)
                    .Include(m => m.Goals);
            }
            
            var metric = await query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            
            if (metric == null)
            {
                throw new EntityNotFoundException(typeof(TrackedMetric), id);
            }
            
            return metric;
        }

        public override async Task<TrackedMetric> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(m => m.DataPoints)
                    .Include(m => m.Goals);
            }
            
            return await query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public override async Task<IQueryable<TrackedMetric>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(m => m.DataPoints)
                .Include(m => m.Goals);
        }
    }
}
