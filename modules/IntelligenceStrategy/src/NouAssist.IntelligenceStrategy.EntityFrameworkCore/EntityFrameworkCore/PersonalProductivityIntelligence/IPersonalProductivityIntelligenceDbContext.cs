using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore;

/// <summary>
/// Interface for the PersonalProductivityIntelligence DbContext
/// </summary>
[ConnectionStringName(PersonalProductivityIntelligenceDbProperties.ConnectionStringName)]
public interface IPersonalProductivityIntelligenceDbContext : IEfCoreDbContext
{
    /// <summary>
    /// User productivity profiles
    /// </summary>
    DbSet<UserProductivityProfile> UserProductivityProfiles { get; }
    
    /// <summary>
    /// Prediction instances
    /// </summary>
    DbSet<PredictionInstance> PredictionInstances { get; }
    
    /// <summary>
    /// Recommendation instances
    /// </summary>
    DbSet<RecommendationInstance> RecommendationInstances { get; }
    
    /// <summary>
    /// Personal automations
    /// </summary>
    DbSet<PersonalAutomation> PersonalAutomations { get; }
    
    /// <summary>
    /// Performance snapshots
    /// </summary>
    DbSet<PerformanceSnapshot> PerformanceSnapshots { get; }
}
