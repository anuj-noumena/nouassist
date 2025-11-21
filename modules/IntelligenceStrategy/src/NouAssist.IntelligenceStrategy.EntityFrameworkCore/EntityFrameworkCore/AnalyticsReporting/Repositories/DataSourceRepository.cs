using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Analytics.EntityFrameworkCore;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Analytics.Repositories
{
    /// <summary>
    /// Implementation of the data source repository
    /// </summary>
    public class DataSourceRepository :
        EfCoreRepository<AnalyticsDbContext, DataSource, Guid>,
        IDataSourceRepository
    {
        public DataSourceRepository(IDbContextProvider<AnalyticsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DataSource> FindByNameAsync(string name, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(d => d.TenantId == tenantId && d.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<DataSource[]> GetByTypeAsync(DataSourceType type, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(d => d.TenantId == tenantId && d.Type == type)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<DataSource[]> GetActiveDataSourcesAsync(Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(d => d.TenantId == tenantId && d.IsActive)
                .ToArrayAsync(cancellationToken);
        }
        
        public override async Task<IQueryable<DataSource>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(d => d.TransformationSteps)
                .Include(d => d.AggregatedDataViews);
        }
    }
}
