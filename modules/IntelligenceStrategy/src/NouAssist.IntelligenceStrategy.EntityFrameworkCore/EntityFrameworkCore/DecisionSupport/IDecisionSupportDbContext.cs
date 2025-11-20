using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore
{
    /// <summary>
    /// Database context interface for the Decision Support bounded context
    /// </summary>
    [ConnectionStringName(DecisionSupportDbProperties.ConnectionStringName)]
    public interface IDecisionSupportDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// Strategic initiative proposals in the system
        /// </summary>
        DbSet<StrategicInitiativeProposal> StrategicInitiativeProposals { get; }
        
        /// <summary>
        /// Decision model instances in the system
        /// </summary>
        DbSet<DecisionModelInstance> DecisionModelInstances { get; }
    }
}
