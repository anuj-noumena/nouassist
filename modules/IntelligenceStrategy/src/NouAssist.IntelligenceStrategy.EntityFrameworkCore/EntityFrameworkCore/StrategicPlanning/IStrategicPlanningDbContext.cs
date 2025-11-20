using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for Strategic Planning module
    /// </summary>
    [ConnectionStringName(StrategicPlanningDbProperties.ConnectionStringName)]
    public interface IStrategicPlanningDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root in Strategic Planning module
        DbSet<StrategicGoalDefinition> StrategicGoals { get; }
        DbSet<CrisisManagementPlan> CrisisManagementPlans { get; }
        DbSet<SustainableGrowthFramework> SustainableGrowthFrameworks { get; }
        DbSet<IntegratedHorizonPlan> IntegratedHorizonPlans { get; }
        DbSet<OpportunityManagementRecord> OpportunityManagementRecords { get; }
        DbSet<TransitionPlan> TransitionPlans { get; }
    }
}
