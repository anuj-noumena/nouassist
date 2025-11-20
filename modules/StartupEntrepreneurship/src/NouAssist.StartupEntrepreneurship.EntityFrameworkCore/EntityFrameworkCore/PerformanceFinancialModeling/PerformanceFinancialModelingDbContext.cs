using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore
{
    [ConnectionStringName(PerformanceFinancialModelingDbProperties.ConnectionStringName)]
    public class PerformanceFinancialModelingDbContext 
        : AbpDbContext<PerformanceFinancialModelingDbContext>, IPerformanceFinancialModelingDbContext
    {
        // Aggregate Roots
        public DbSet<TrackedMetric> TrackedMetrics { get; set; }
        public DbSet<FinancialModel> FinancialModels { get; set; }

        // Supporting Entities
        public DbSet<MetricDefinition> MetricDefinitions { get; set; }
        public DbSet<MetricCategory> MetricCategories { get; set; }

        public PerformanceFinancialModelingDbContext(DbContextOptions<PerformanceFinancialModelingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePerformanceFinancialModeling(this);
        }
    }
}
