using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class SubscriptionRuleConfiguration : IEntityTypeConfiguration<SubscriptionRule>
    {
        public void Configure(EntityTypeBuilder<SubscriptionRule> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "SubscriptionRules", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Channel)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IsEnabled)
                .IsRequired();

            // Value Object Mapping - NotificationEventTrigger
            builder.OwnsOne(x => x.EventTrigger, triggerBuilder =>
            {
                triggerBuilder.Property(t => t.EventType)
                    .HasColumnName("EventType")
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(50);

                // Nested Value Object - TargetResource
                triggerBuilder.OwnsOne(t => t.TargetResource, resourceBuilder =>
                {
                    resourceBuilder.Property(r => r.Type)
                        .HasColumnName("ResourceType")
                        .HasConversion<string>()
                        .IsRequired()
                        .HasMaxLength(50);

                    resourceBuilder.Property(r => r.Id)
                        .HasColumnName("ResourceId");
                });
            });

            // Indexes
            builder.HasIndex("EventTrigger_EventType");
            builder.HasIndex("EventTrigger_TargetResource_Type", "EventTrigger_TargetResource_Id");
            builder.HasIndex("Channel", "IsEnabled");
        }
    }
}
