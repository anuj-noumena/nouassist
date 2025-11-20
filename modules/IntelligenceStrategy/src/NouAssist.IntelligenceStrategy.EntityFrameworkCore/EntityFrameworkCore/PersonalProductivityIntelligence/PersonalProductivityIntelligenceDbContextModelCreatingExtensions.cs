using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore;

/// <summary>
/// Extensions for configuring the PersonalProductivityIntelligence module entities
/// </summary>
public static class PersonalProductivityIntelligenceDbContextModelCreatingExtensions
{
    /// <summary>
    /// Applies PersonalProductivityIntelligence module configurations to the model builder
    /// </summary>
    public static void ConfigurePersonalProductivityIntelligence(
        this ModelBuilder builder,
        PersonalProductivityIntelligenceDbContext dbContext)
    {
        Check.NotNull(builder, nameof(builder));

        // Apply all IEntityTypeConfiguration classes defined in this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(PersonalProductivityIntelligenceDbContext).Assembly);

        // Any additional module-specific model configurations can go here
    }
}
