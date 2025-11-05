using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Workflow.Entities;

namespace NouAssist.Workflow.EntityFrameworkCore.Configurations
{
    public class WorkflowSchemeMappingConfiguration : IEntityTypeConfiguration<WorkflowSchemeMapping>
    {
        public void Configure(EntityTypeBuilder<WorkflowSchemeMapping> builder)
        {
            builder.ToTable(WorkflowDbProperties.DbTablePrefix + "SchemeMappings", WorkflowDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowSchemeId)
                .IsRequired();

            builder.Property(x => x.TaskTypeId)
                .IsRequired();

            builder.Property(x => x.WorkflowId)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.WorkflowSchemeId);
            builder.HasIndex(x => new { x.WorkflowSchemeId, x.TaskTypeId });
        }
    }
}
