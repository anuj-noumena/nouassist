using Microsoft.EntityFrameworkCore;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.EntityFrameworkCore.EntityConfigurations;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Analytics.EntityFrameworkCore
{
    [ConnectionStringName("Analytics")]
    public class AnalyticsDbContext : AbpDbContext<AnalyticsDbContext>
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<ReportConfiguration> ReportConfigurations { get; set; }
        public DbSet<KpiDefinition> KpiDefinitions { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<DashboardTemplate> DashboardTemplates { get; set; }

        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside here */
            builder.ApplyConfiguration(new DashboardConfiguration());
            builder.ApplyConfiguration(new ReportConfigurationConfiguration());
            builder.ApplyConfiguration(new KpiDefinitionConfiguration());
            builder.ApplyConfiguration(new DataSourceConfiguration());
            builder.ApplyConfiguration(new DashboardTemplateConfiguration());
        }
    }
}
