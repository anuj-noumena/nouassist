using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Configurations
{
    public class WorkflowSchemeAggregateConfiguration : IEntityTypeConfiguration<WorkflowScheme>
    {
        public void Configure(EntityTypeBuilder<WorkflowScheme> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkflowDbProperties.DbTablePrefix + "Schemes", WorkflowDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.DefaultWorkflowId);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDraft)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.Mappings)
                .WithOne()
                .HasForeignKey(m => m.WorkflowSchemeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => new { x.ProjectId, x.IsActive });
            builder.HasIndex(x => x.Name);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
