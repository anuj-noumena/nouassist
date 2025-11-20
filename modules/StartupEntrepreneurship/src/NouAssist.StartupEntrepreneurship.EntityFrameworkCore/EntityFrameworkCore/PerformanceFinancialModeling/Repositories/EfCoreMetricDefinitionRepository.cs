using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories
{
    public class EfCoreMetricDefinitionRepository 
        : EfCoreRepository<IPerformanceFinancialModelingDbContext, MetricDefinition, Guid>
    {
        public EfCoreMetricDefinitionRepository(
            IDbContextProvider<IPerformanceFinancialModelingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<MetricDefinition> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(d => d.Name == name && d.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<MetricDefinition>> GetByTypeAsync(MetricType type, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(d => d.Type == type && d.TenantId == tenantId && d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MetricDefinition>> GetActiveDefinitionsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(d => d.IsActive && d.TenantId == tenantId)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MetricDefinition>> GetByCategoryAsync(Guid categoryId, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(d => d.CategoryId == categoryId && d.TenantId == tenantId && d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
    }
}
