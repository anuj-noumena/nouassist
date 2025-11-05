using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Workflow.Entities;

namespace NouAssist.Workflow.EntityFrameworkCore.Configurations
{
    public class WorkflowStatusConfiguration : IEntityTypeConfiguration<WorkflowStatus>
    {
        public void Configure(EntityTypeBuilder<WorkflowStatus> builder)
        {
            builder.ToTable(WorkflowDbProperties.DbTablePrefix + "Statuses", WorkflowDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.Property(x => x.IsInitial)
                .IsRequired();

            builder.Property(x => x.IsResolution)
                .IsRequired();

            // Collection: Properties (stored as JSON)
            builder.OwnsOne(x => x.Properties, propsBuilder =>
            {
                propsBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => new { x.WorkflowId, x.OrderIndex });
            builder.HasIndex(x => new { x.WorkflowId, x.Name });
        }
    }
}
