using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the WorkflowDefinition aggregate
    /// </summary>
    public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "WorkflowDefinitions", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(WorkflowEngineConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(WorkflowEngineConsts.MaxDescriptionLength);

            builder.Property(x => x.Category)
                .HasMaxLength(128);

            builder.Property(x => x.Documentation)
                .HasMaxLength(WorkflowEngineConsts.MaxDocumentationLength);

            // Value Object Mapping
            builder.OwnsOne(x => x.PerformanceMetrics, performanceBuilder =>
            {
                performanceBuilder.Property(p => p.AverageCycleTimeMinutes)
                    .HasColumnName("PerformanceAverageCycleTime");
                
                performanceBuilder.Property(p => p.BottleneckScore)
                    .HasColumnName("PerformanceBottleneckScore");
                
                performanceBuilder.Property(p => p.BottleneckActivityId)
                    .HasColumnName("PerformanceBottleneckActivityId");
                
                performanceBuilder.Property(p => p.SampleSize)
                    .HasColumnName("PerformanceSampleSize");
                
                performanceBuilder.Property(p => p.CalculationTime)
                    .HasColumnName("PerformanceCalculationTime");
            });

            // Relationships
            builder.HasMany(x => x.Versions)
                   .WithOne()
                   .HasForeignKey(v => v.WorkflowDefinitionId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        }
    }
}
