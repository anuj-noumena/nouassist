using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Entities;

namespace NouAssist.TimeManagement.EntityFrameworkCore.Calendar.EntityFrameworkCore.Configurations
{
    public class TimeblockTaskConfiguration : IEntityTypeConfiguration<TimeblockTask>
    {
        public void Configure(EntityTypeBuilder<TimeblockTask> builder)
        {
            builder.ToTable(TimeManagementDbProperties.DbTablePrefix + "TimeblockTasks", TimeManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TimeblockId)
                .IsRequired();

            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.EstimatedMinutes)
                .IsRequired();

            builder.Property(x => x.ActualMinutes)
                .IsRequired();

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.TimeblockId);
            builder.HasIndex(x => x.TaskInstanceId);
        }
    }
}
