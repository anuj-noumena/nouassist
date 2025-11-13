using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class AcceptanceCriterionCompletionConfiguration : IEntityTypeConfiguration<AcceptanceCriterionCompletion>
    {
        public void Configure(EntityTypeBuilder<AcceptanceCriterionCompletion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "AcceptanceCriterionCompletions",
                          TasksDbProperties.DbSchema);

            // Composite key
            builder.HasKey(x => new { x.TaskInstanceId, x.CriterionId });

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.CriterionId)
                .IsRequired();

            builder.Property(x => x.IsMet)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.MetAt);

            builder.Property(x => x.MetBy);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId);
            builder.HasIndex(x => x.CriterionId);
            builder.HasIndex(x => new { x.TaskInstanceId, x.IsMet });

            // Concurrency
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);
        }
    }
}
