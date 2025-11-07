using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Linq.Dynamic.Core;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Configurations
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

            // Collections stored as JSON
            builder.OwnsMany(x => x.VisibleInStatusIds, visiBuilder =>
            {
                visiBuilder.ToJson();
            });

            builder.OwnsMany(x => x.RequiredInStatusIds, reqBuilder =>
            {
                reqBuilder.ToJson();
            });

            builder.OwnsMany(x => x.ReadOnlyInStatusIds, roBuilder =>
            {
                roBuilder.ToJson();
            });

            builder.OwnsMany(x => x.RequiredForTransitionIds, rftBuilder =>
            {
                rftBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.WorkflowSchemeId);
            builder.HasIndex(x => new { x.WorkflowSchemeId, x.CustomFieldDefinitionId });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
