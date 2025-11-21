using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the ConditionDefinition entity
    /// </summary>
    public class ConditionDefinitionConfiguration : IEntityTypeConfiguration<ConditionDefinition>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<ConditionDefinition> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "ConditionDefinitions", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(WorkflowEngineConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(WorkflowEngineConsts.MaxDescriptionLength);

            // Value Object Mapping
            builder.OwnsOne(x => x.Expression, expressionBuilder =>
            {
                expressionBuilder.Property(e => e.Expression)
                    .HasColumnName("ExpressionText")
                    .HasMaxLength(WorkflowEngineConsts.MaxExpressionLength)
                    .IsRequired();
                
                expressionBuilder.Property(e => e.Language)
                    .HasColumnName("ExpressionLanguage")
                    .HasMaxLength(50)
                    .IsRequired();
                
                expressionBuilder.Property(e => e.Description)
                    .HasColumnName("ExpressionDescription")
                    .HasMaxLength(WorkflowEngineConsts.MaxDescriptionLength);
            });

            // Indexing
            builder.HasIndex(x => new { x.WorkflowDefinitionId, x.Name });
            builder.HasIndex(x => x.SourceActivityId);
            builder.HasIndex(x => x.TargetActivityId);
        }
    }
}
