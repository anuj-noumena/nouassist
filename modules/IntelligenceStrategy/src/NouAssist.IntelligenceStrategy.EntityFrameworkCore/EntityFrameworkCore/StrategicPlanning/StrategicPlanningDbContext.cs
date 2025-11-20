using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for Strategic Planning module
    /// </summary>
    [ConnectionStringName(StrategicPlanningDbProperties.ConnectionStringName)]
    public class StrategicPlanningDbContext 
        : AbpDbContext<StrategicPlanningDbContext>, IStrategicPlanningDbContext 
    {
        // Implement DbSet properties from the interface
        public DbSet<StrategicGoalDefinition> StrategicGoals { get; set; }
        public DbSet<CrisisManagementPlan> CrisisManagementPlans { get; set; }
        public DbSet<SustainableGrowthFramework> SustainableGrowthFrameworks { get; set; }
        public DbSet<IntegratedHorizonPlan> IntegratedHorizonPlans { get; set; }
        public DbSet<OpportunityManagementRecord> OpportunityManagementRecords { get; set; }
        public DbSet<TransitionPlan> TransitionPlans { get; set; }

        public StrategicPlanningDbContext(DbContextOptions<StrategicPlanningDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder); 

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureStrategicPlanning(this); 
        }
    }
}
