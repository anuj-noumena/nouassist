using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore
{
    /// <summary>
    /// Database context for the Decision Support bounded context
    /// </summary>
    [ConnectionStringName(DecisionSupportDbProperties.ConnectionStringName)]
    public class DecisionSupportDbContext : AbpDbContext<DecisionSupportDbContext>, IDecisionSupportDbContext
    {
        /// <summary>
        /// Strategic initiative proposals in the system
        /// </summary>
        public DbSet<StrategicInitiativeProposal> StrategicInitiativeProposals { get; set; }
        
        /// <summary>
        /// Decision model instances in the system
        /// </summary>
        public DbSet<DecisionModelInstance> DecisionModelInstances { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DecisionSupportDbContext"/>
        /// </summary>
        public DecisionSupportDbContext(DbContextOptions<DecisionSupportDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base method first to apply ABP conventions
            base.OnModelCreating(builder);
            
            // Apply module-specific configurations
            builder.ConfigureDecisionSupport(this);
        }
    }
}
