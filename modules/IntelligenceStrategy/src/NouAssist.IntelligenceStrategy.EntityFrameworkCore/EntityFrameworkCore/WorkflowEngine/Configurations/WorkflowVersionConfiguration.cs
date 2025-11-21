using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the WorkflowVersion entity
    /// </summary>
    public class WorkflowVersionConfiguration : IEntityTypeConfiguration<WorkflowVersion>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<WorkflowVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "WorkflowVersions", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key (composite key)
            builder.HasKey(x => new { x.WorkflowDefinitionId, x.VersionNumber });

            // Properties
            builder.Property(x => x.Comments)
                .HasMaxLength(1000);

            // Value Object Mapping
            builder.OwnsOne(x => x.BpmnDiagram, bpmnBuilder =>
            {
                bpmnBuilder.Property(b => b.XmlContent)
                    .HasColumnName("BpmnXmlContent")
                    .HasMaxLength(WorkflowEngineConsts.MaxBpmnDiagramLength);
                
                bpmnBuilder.Property(b => b.BpmnVersion)
                    .HasColumnName("BpmnVersion")
                    .HasMaxLength(20);
                
                bpmnBuilder.Property(b => b.LastUpdated)
                    .HasColumnName("BpmnLastUpdated");
            });

            // Relationships
            builder.HasMany(x => x.Activities)
                   .WithOne()
                   .HasForeignKey(a => a.WorkflowDefinitionId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Conditions)
                   .WithOne()
                   .HasForeignKey(c => c.WorkflowDefinitionId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
