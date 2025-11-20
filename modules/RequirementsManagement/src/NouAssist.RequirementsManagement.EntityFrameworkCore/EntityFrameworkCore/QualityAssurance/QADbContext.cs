using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    [ConnectionStringName("QA")]
    public class QADbContext : AbpDbContext<QADbContext>, IQADbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<TestPlan> TestPlans { get; set; }
        public DbSet<TestCycle> TestCycles { get; set; }
        public DbSet<DefectReport> DefectReports { get; set; }
        public DbSet<QualityMetricSnapshot> QualityMetricSnapshots { get; set; }

        public QADbContext(DbContextOptions<QADbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureQA();
        }
    }
}
