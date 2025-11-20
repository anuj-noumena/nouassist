using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the WorkPatternModel entity
/// </summary>
public class WorkPatternModelConfiguration : IEntityTypeConfiguration<WorkPatternModel>
{
    public void Configure(EntityTypeBuilder<WorkPatternModel> builder)
    {
        // Apply ABP base conventions
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "WorkPatternModels",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserProductivityProfileId).IsRequired();
        builder.Property(x => x.PatternDescription).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.PatternConfidence).IsRequired();
        builder.Property(x => x.ModelData).IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.CreatedAt).IsRequired();

        // Relationships - the parent-child relationship is defined in UserProductivityProfileConfiguration

        // Indexing
        builder.HasIndex(x => x.UserProductivityProfileId);
    }
}
