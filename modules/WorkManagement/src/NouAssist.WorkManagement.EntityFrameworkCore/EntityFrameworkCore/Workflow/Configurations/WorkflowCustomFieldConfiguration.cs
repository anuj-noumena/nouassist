using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow.Configurations
{
    public class WorkflowCustomFieldConfiguration : IEntityTypeConfiguration<WorkflowCustomField>
    {
        public void Configure(EntityTypeBuilder<WorkflowCustomField> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkflowDbProperties.DbTablePrefix + "CustomFields", WorkflowDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowSchemeId)
                .IsRequired();

            builder.Property(x => x.CustomFieldDefinitionId)
                .IsRequired();

            builder.Property(x => x.IsRequired)
                .IsRequired();

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(500);

            builder.Property(x => x.DisplayOrder)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // JSON strings for rules
            builder.Property(x => x.VisibilityRules)
                .HasColumnType("nvarchar(max)"); // or "text" for PostgreSQL

            builder.Property(x => x.ValidationRules)
                .HasColumnType("nvarchar(max)"); // or "text" for PostgreSQL

            // Indexes
            builder.HasIndex(x => x.WorkflowSchemeId);
            builder.HasIndex(x => new { x.WorkflowSchemeId, x.CustomFieldDefinitionId });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}