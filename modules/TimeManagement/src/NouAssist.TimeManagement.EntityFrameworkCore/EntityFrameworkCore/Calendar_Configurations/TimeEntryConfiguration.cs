using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Configurations
{
    public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "TimeEntries", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.TaskInstanceId);
            builder.Property(x => x.ProjectId);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.StartedAt)
                .IsRequired();

            builder.Property(x => x.EndedAt);
            builder.Property(x => x.DurationMinutes);

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            builder.Property(x => x.IsBillable)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Collection: Tags (stored as JSON array)
            builder.OwnsMany(x => x.Tags, tagBuilder =>
            {
                tagBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.TaskInstanceId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => new { x.StartedAt, x.EndedAt });
            builder.HasIndex(x => new { x.OwnerId, x.StartedAt });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
