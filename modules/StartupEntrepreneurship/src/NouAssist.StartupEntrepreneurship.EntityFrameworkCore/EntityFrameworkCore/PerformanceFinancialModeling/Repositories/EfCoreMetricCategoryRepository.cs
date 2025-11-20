using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories
{
    public class EfCoreMetricCategoryRepository 
        : EfCoreRepository<IPerformanceFinancialModelingDbContext, MetricCategory, Guid>
    {
        public EfCoreMetricCategoryRepository(
            IDbContextProvider<IPerformanceFinancialModelingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<MetricCategory> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.Name == name && c.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<MetricCategory>> GetActiveCategoriesAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.IsActive && c.TenantId == tenantId)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MetricCategory>> GetOrderedCategoriesAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.TenantId == tenantId)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }
    }
}
