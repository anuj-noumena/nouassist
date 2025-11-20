using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the UserProductivityProfile aggregate
/// </summary>
public class UserProductivityProfileConfiguration : IEntityTypeConfiguration<UserProductivityProfile>
{
    public void Configure(EntityTypeBuilder<UserProductivityProfile> builder)
    {
        // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency)
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "UserProductivityProfiles",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key (automatically handled by ConfigureByConvention)
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CognitiveLoadLevel).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.CognitiveLoadValue).IsRequired();
        builder.Property(x => x.CognitiveLoadLastUpdated).IsRequired();

        // WorkPattern relationship (one-to-one)
        builder.HasOne(x => x.WorkPattern)
            .WithOne()
            .HasForeignKey<WorkPatternModel>(w => w.UserProductivityProfileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // UserPreferenceSetting relationship (one-to-many)
        builder.OwnsMany(x => x.PreferenceSettings, preferencesBuilder =>
        {
            preferencesBuilder.WithOwner().HasForeignKey("UserProductivityProfileId");
            preferencesBuilder.HasKey("UserProductivityProfileId", "Key");
            preferencesBuilder.Property(p => p.Key).HasMaxLength(128).IsRequired();
            preferencesBuilder.Property(p => p.Value).HasMaxLength(512);
            preferencesBuilder.Property(p => p.LastUpdated).IsRequired();
            preferencesBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "UserPreferenceSettings", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // PerformanceDataPoint relationship (one-to-many)
        builder.OwnsMany(x => x.PerformanceData, performanceBuilder =>
        {
            performanceBuilder.WithOwner().HasForeignKey("UserProductivityProfileId");
            performanceBuilder.HasKey("UserProductivityProfileId", "MetricName", "Timestamp");
            performanceBuilder.Property(p => p.MetricName).HasMaxLength(128).IsRequired();
            performanceBuilder.Property(p => p.Value).IsRequired();
            performanceBuilder.Property(p => p.Timestamp).IsRequired();
            performanceBuilder.Property(p => p.Context).HasMaxLength(256);
            performanceBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PerformanceDataPoints", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // TaskCompletionRecord relationship (one-to-many)
        builder.OwnsMany(x => x.TaskCompletionRecords, taskBuilder =>
        {
            taskBuilder.WithOwner().HasForeignKey("UserProductivityProfileId");
            taskBuilder.HasKey(t => t.Id);
            taskBuilder.Property(t => t.TaskName).HasMaxLength(256).IsRequired();
            taskBuilder.Property(t => t.EstimatedDuration).IsRequired();
            taskBuilder.Property(t => t.ActualDuration).IsRequired();
            taskBuilder.Property(t => t.CompletedAt).IsRequired();
            
            // TaskContext value object mapping
            taskBuilder.OwnsOne(t => t.TaskContext, contextBuilder =>
            {
                contextBuilder.Property(c => c.ProjectId).HasColumnName("ProjectId");
                contextBuilder.Property(c => c.TeamId).HasColumnName("TeamId");
                contextBuilder.Property(c => c.Location).HasColumnName("Location").HasMaxLength(256);
                contextBuilder.Property(c => c.EnergyLevel).HasColumnName("EnergyLevel").HasMaxLength(50);
                contextBuilder.Property(c => c.FocusLevel).HasColumnName("FocusLevel").HasMaxLength(50);
                
                // Store tags as comma-separated string
                contextBuilder.Property<string>("TagsString")
                    .HasColumnName("Tags")
                    .HasMaxLength(1024);
                
                // Custom value converter for the tags collection
                contextBuilder.Ignore(c => c.Tags);
            });
            
            taskBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "TaskCompletionRecords", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // External context references (one-to-many)
        builder.OwnsMany(x => x.ExternalContextRefs, contextBuilder =>
        {
            contextBuilder.WithOwner().HasForeignKey("UserProductivityProfileId");
            contextBuilder.HasKey("UserProductivityProfileId", "EntityType", "EntityId");
            contextBuilder.Property(c => c.EntityType).HasMaxLength(128).IsRequired();
            contextBuilder.Property(c => c.EntityId).IsRequired();
            
            // Store properties as JSON
            contextBuilder.Property(c => c.Properties)
                .HasColumnType("jsonb");
            
            contextBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "ExternalContextRefs", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // Indexing
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => x.CognitiveLoadLevel);
    }
}
