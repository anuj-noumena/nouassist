using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore
{
    [ConnectionStringName(RequirementsAnalysisToolsDbProperties.ConnectionStringName)]
    public interface IRequirementsAnalysisToolsDbContext : IEfCoreDbContext
    {
        DbSet<AnalyzedRequirement> AnalyzedRequirements { get; }
        DbSet<GlossaryTerm> GlossaryTerms { get; }
        DbSet<PrioritizationActivity> PrioritizationActivities { get; }
    }
}
