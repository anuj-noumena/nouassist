using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the PersonalAutomation aggregate
/// </summary>
public class PersonalAutomationConfiguration : IEntityTypeConfiguration<PersonalAutomation>
{
    public void Configure(EntityTypeBuilder<PersonalAutomation> builder)
    {
        // Apply ABP base conventions
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PersonalAutomations",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(PersonalProductivityIntelligenceConsts.MaxNameLength);
        builder.Property(x => x.Description).HasMaxLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength);
        builder.Property(x => x.IsEnabled).IsRequired().HasDefaultValue(true);

        // AutomationTrigger relationship (one-to-many)
        builder.OwnsMany(x => x.Triggers, triggerBuilder =>
        {
            triggerBuilder.WithOwner().HasForeignKey("AutomationId");
            triggerBuilder.HasKey(t => t.Id);
            
            triggerBuilder.Property(t => t.TriggerType).IsRequired().HasConversion<string>().HasMaxLength(50);
            triggerBuilder.Property(t => t.TriggerData).IsRequired().HasColumnType("jsonb");
            triggerBuilder.Property(t => t.TriggerDescription).HasMaxLength(256);
            
            triggerBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "AutomationTriggers", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // AutomationAction relationship (one-to-many)
        builder.OwnsMany(x => x.Actions, actionBuilder =>
        {
            actionBuilder.WithOwner().HasForeignKey("AutomationId");
            actionBuilder.HasKey(a => a.Id);
            
            actionBuilder.Property(a => a.ActionType).IsRequired().HasMaxLength(50);
            actionBuilder.Property(a => a.ActionData).IsRequired().HasColumnType("jsonb");
            actionBuilder.Property(a => a.Order).IsRequired();
            actionBuilder.Property(a => a.ActionDescription).HasMaxLength(256);
            
            actionBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "AutomationActions", PersonalProductivityIntelligenceDbProperties.DbSchema);
            
            // Indexing
            actionBuilder.HasIndex(a => a.Order);
        });

        // Indexing
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.IsEnabled);
        builder.HasIndex(x => new { x.UserId, x.IsEnabled });
    }
}
