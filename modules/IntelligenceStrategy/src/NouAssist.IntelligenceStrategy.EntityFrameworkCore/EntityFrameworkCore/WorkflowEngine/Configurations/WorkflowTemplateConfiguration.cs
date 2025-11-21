using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the WorkflowTemplate aggregate
    /// </summary>
    public class WorkflowTemplateConfiguration : IEntityTypeConfiguration<WorkflowTemplate>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<WorkflowTemplate> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "WorkflowTemplates", 
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

            // Collection properties
            builder.Property(x => x.Tags)
                .HasColumnName("Tags")
                .HasJsonConversion();

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.IsPublic);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        }
    }
}
