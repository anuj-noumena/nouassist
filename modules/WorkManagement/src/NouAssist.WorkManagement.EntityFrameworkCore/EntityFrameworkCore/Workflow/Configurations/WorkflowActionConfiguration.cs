using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Workflow.Entities;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow.Configurations
{
    public class WorkflowActionConfiguration : IEntityTypeConfiguration<WorkflowAction>
    {
        public void Configure(EntityTypeBuilder<WorkflowAction> builder)
        {
            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "Actions", WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransitionId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.ActionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.Property(x => x.IsEnabled)
                .IsRequired();

            builder.Property(x => x.IsAsync)
                .IsRequired();

            builder.Property(x => x.ContinueOnError)
                .IsRequired();

            // Collections stored as JSON
            builder.OwnsOne(x => x.Configuration, configBuilder =>
            {
                configBuilder.ToJson();
            });

            // Conditions is a Dictionary<string, object>
            builder.OwnsOne(x => x.Conditions, conditionsBuilder =>
            {
                conditionsBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.TransitionId);
            builder.HasIndex(x => new { x.TransitionId, x.OrderIndex });
        }
    }
}