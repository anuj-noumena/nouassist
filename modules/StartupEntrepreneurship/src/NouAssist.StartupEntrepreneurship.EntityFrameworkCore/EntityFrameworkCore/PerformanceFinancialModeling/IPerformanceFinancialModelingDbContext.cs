using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore
{
    [ConnectionStringName(PerformanceFinancialModelingDbProperties.ConnectionStringName)]
    public interface IPerformanceFinancialModelingDbContext : IEfCoreDbContext
    {
        // Aggregate Roots
        DbSet<TrackedMetric> TrackedMetrics { get; }
        DbSet<FinancialModel> FinancialModels { get; }

        // Supporting Entities 
        DbSet<MetricDefinition> MetricDefinitions { get; }
        DbSet<MetricCategory> MetricCategories { get; }
    }
}
