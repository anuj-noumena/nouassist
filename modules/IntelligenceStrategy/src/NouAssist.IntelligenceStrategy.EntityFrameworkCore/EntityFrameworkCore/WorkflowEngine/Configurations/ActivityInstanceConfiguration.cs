using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the ActivityInstance entity
    /// </summary>
    public class ActivityInstanceConfiguration : IEntityTypeConfiguration<ActivityInstance>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<ActivityInstance> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "ActivityInstances", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(WorkflowEngineConsts.MaxNameLength);

            builder.Property(x => x.ActivityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping
            builder.OwnsOne(x => x.Assignee, assigneeBuilder =>
            {
                assigneeBuilder.Property(a => a.Type)
                    .HasColumnName("AssigneeType")
                    .HasConversion<string>()
                    .HasMaxLength(50);
                
                assigneeBuilder.Property(a => a.Id)
                    .HasColumnName("AssigneeId");
                
                assigneeBuilder.Property(a => a.DisplayName)
                    .HasColumnName("AssigneeDisplayName")
                    .HasMaxLength(256);
            });

            // Properties Dictionary
            builder.Property(x => x.Properties)
                .HasColumnName("Properties")
                .HasJsonConversion();

            // Indexing
            builder.HasIndex(x => x.WorkflowInstanceId);
            builder.HasIndex(x => x.ActivityDefinitionId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.StartTime);
        }
    }
}
