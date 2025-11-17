using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Workflow.Entities;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow.Configurations
{
    public class WorkflowTransitionConfiguration : IEntityTypeConfiguration<WorkflowTransition>
    {
        public void Configure(EntityTypeBuilder<WorkflowTransition> builder)
        {
            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "Transitions",
                WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.FromStatusId)
                .IsRequired();

            builder.Property(x => x.ToStatusId)
                .IsRequired();

            builder.Property(x => x.Purpose)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.RequiresComment)
                .IsRequired();

            builder.Property(x => x.RequiresResolution)
                .IsRequired();

            builder.Property(x => x.IsGlobal)
                .IsRequired();

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.Property(x => x.ButtonLabel)
                .HasMaxLength(100);

            builder.Property(x => x.ButtonColor)
                .HasMaxLength(20);

            // Collections stored as JSON
            // Conditions is now a List<WorkflowCondition> value objects
            builder.OwnsMany(x => x.Conditions, conditionsBuilder =>
            {
                conditionsBuilder.ToJson();
                conditionsBuilder.Property(c => c.Key).IsRequired();
                conditionsBuilder.Property(c => c.Operator).HasMaxLength(50);
            });

            builder.OwnsOne(x => x.Validators, validatorsBuilder => { validatorsBuilder.ToJson(); });

            // Relationships
            builder.HasMany(x => x.Actions)
                .WithOne()
                .HasForeignKey(a => a.TransitionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => new { x.WorkflowId, x.FromStatusId });
            builder.HasIndex(x => new { x.WorkflowId, x.ToStatusId });
            builder.HasIndex(x => new { x.WorkflowId, x.FromStatusId, x.ToStatusId });
            builder.HasIndex(x => x.Purpose);
        }
    }
}