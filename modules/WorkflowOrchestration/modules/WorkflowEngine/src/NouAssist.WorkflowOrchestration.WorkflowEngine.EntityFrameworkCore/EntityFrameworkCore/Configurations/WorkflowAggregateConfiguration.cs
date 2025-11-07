using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Configurations
{
    public class WorkflowAggregateConfiguration : IEntityTypeConfiguration<Aggregates.Workflow>
    {
        public void Configure(EntityTypeBuilder<Aggregates.Workflow> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkflowDbProperties.DbTablePrefix + "Workflows", WorkflowDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.InitialStatusId)
                .IsRequired();

            builder.Property(x => x.IsDefault)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Collection: Properties (stored as JSON)
            builder.OwnsOne(x => x.Properties, propsBuilder =>
            {
                propsBuilder.ToJson();
            });

            // Relationships
            builder.HasMany(x => x.Statuses)
                .WithOne()
                .HasForeignKey(s => s.WorkflowId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Transitions)
                .WithOne()
                .HasForeignKey(t => t.WorkflowId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsDefault);
            builder.HasIndex(x => x.IsActive);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
