using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskActionStepConfiguration : IEntityTypeConfiguration<TaskActionStep>
    {
        public void Configure(EntityTypeBuilder<TaskActionStep> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskActionSteps",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxStepTitleLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(TasksDbConsts.MaxStepNotesLength);

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Sequence)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => new { x.TaskId, x.Sequence });

            // Concurrency
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);
        }
    }
}
