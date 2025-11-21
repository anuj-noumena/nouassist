using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories
{
    public class EfCoreSensitivityRunRepository 
        : EfCoreRepository<IPerformanceFinancialModelingDbContext, SensitivityRun, Guid>, ISensitivityRunRepository
    {
        public EfCoreSensitivityRunRepository(
            IDbContextProvider<IPerformanceFinancialModelingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SensitivityRun>> GetByModelAsync(Guid modelId, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.ModelId == modelId)
                .OrderByDescending(r => r.RunDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SensitivityRun>> GetByScenarioAsync(Guid scenarioId, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.ScenarioId == scenarioId)
                .OrderByDescending(r => r.RunDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SensitivityRun>> GetRecentRunsAsync(int days = 30, CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.RunDate >= cutoffDate)
                .OrderByDescending(r => r.RunDate)
                .ToListAsync(cancellationToken);
        }
    }
}

// Update: src/NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore/EntityFrameworkCore/PerformanceFinancialModelingEntityFrameworkCoreModule.cs
[DependsOn(
    typeof(PerformanceFinancialModelingDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSQLModule)
)]
public class PerformanceFinancialModelingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PerformanceFinancialModelingDbContext>(options =>
        {
            // Register CUSTOM repositories here for aggregate roots
            options.AddRepository<TrackedMetric, EfCoreTrackedMetricRepository>();
            options.AddRepository<FinancialModel, EfCoreFinancialModelRepository>();

            // Register default repositories for non-aggregate entities
            options.AddDefaultRepository<MetricDefinition>();
            options.AddDefaultRepository<MetricCategory>();
            options.AddRepository<SensitivityRun, EfCoreSensitivityRunRepository>(); // Added this line

            options.AddDefaultRepositories(includeAllEntities: false);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
    }
}
