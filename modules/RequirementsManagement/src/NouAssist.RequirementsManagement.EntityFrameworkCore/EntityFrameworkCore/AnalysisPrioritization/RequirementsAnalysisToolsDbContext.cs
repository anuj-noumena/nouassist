using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    [ConnectionStringName(RequirementsAnalysisToolsDbProperties.ConnectionStringName)]
    public class RequirementsAnalysisToolsDbContext 
        : AbpDbContext<RequirementsAnalysisToolsDbContext>, IRequirementsAnalysisToolsDbContext
    {
        public DbSet<AnalyzedRequirement> AnalyzedRequirements { get; set; }
        public DbSet<GlossaryTerm> GlossaryTerms { get; set; }
        public DbSet<PrioritizationActivity> PrioritizationActivities { get; set; }

        public RequirementsAnalysisToolsDbContext(DbContextOptions<RequirementsAnalysisToolsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureRequirementsAnalysisTools();
        }
    }
}
