using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore;

/// <summary>
/// DbContext implementation for PersonalProductivityIntelligence module
/// </summary>
[ConnectionStringName(PersonalProductivityIntelligenceDbProperties.ConnectionStringName)]
public class PersonalProductivityIntelligenceDbContext : 
    AbpDbContext<PersonalProductivityIntelligenceDbContext>,
    IPersonalProductivityIntelligenceDbContext
{
    public DbSet<UserProductivityProfile> UserProductivityProfiles { get; set; } = null!;
    public DbSet<PredictionInstance> PredictionInstances { get; set; } = null!;
    public DbSet<RecommendationInstance> RecommendationInstances { get; set; } = null!;
    public DbSet<PersonalAutomation> PersonalAutomations { get; set; } = null!;
    public DbSet<PerformanceSnapshot> PerformanceSnapshots { get; set; } = null!;

    public PersonalProductivityIntelligenceDbContext(DbContextOptions<PersonalProductivityIntelligenceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // IMPORTANT: Call base first to apply ABP conventions
        base.OnModelCreating(builder);

        // Apply module-specific configurations
        builder.ConfigurePersonalProductivityIntelligence(this);
    }
}
