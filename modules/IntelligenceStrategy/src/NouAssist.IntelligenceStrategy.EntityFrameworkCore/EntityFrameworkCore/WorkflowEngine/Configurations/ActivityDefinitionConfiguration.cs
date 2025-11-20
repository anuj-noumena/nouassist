using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the ActivityDefinition entity
    /// </summary>
    public class ActivityDefinitionConfiguration : IEntityTypeConfiguration<ActivityDefinition>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<ActivityDefinition> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "ActivityDefinitions", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(WorkflowEngineConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(WorkflowEngineConsts.MaxDescriptionLength);

            builder.Property(x => x.ActivityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Properties Dictionary
            builder.Property(x => x.Properties)
                .HasColumnName("Properties")
                .HasJsonConversion();

            // Indexing
            builder.HasIndex(x => new { x.WorkflowDefinitionId, x.Name });
            builder.HasIndex(x => x.ActivityType);
        }
    }
}
