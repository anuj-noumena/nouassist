using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using NouAssist.ReflectionAndSupport.Reminders.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore.Configurations
{
    public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RemindersDbProperties.DbTablePrefix + "Reminders", 
                           RemindersDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.RefEntityType)
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(x => x.RemindAt).IsRequired();

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Method)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Value Object: ReminderSchedule
            builder.OwnsOne(x => x.Schedule, schedule =>
            {
                schedule.Property(s => s.BaseDateTime)
                    .HasColumnName("ScheduleBaseDateTime");

                schedule.Property(s => s.MinutesBeforeEvent)
                    .HasColumnName("ScheduleMinutesBefore");

                schedule.Property(s => s.RecurrencePattern)
                    .HasColumnName("RecurrencePattern")
                    .HasConversion<string>()
                    .HasMaxLength(50);

                schedule.Property(s => s.RecurrenceInterval)
                    .HasColumnName("RecurrenceInterval");

                schedule.Property(s => s.RecurrenceEndDate)
                    .HasColumnName("RecurrenceEndDate");

                schedule.Property(s => s.RecurrenceDaysOfWeek)
                    .HasColumnName("RecurrenceDaysOfWeek")
                    .HasConversion(
                        v => string.Join(",", v),
                        v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => (System.DayOfWeek)Enum.Parse(typeof(System.DayOfWeek), s))
                              .ToList()
                    );
            });

            // Value Object: SnoozeInfo
            builder.OwnsOne(x => x.SnoozeInfo, snooze =>
            {
                snooze.Property(s => s.OriginalRemindAt)
                    .HasColumnName("SnoozeOriginalRemindAt");

                snooze.Property(s => s.NewRemindAt)
                    .HasColumnName("SnoozeNewRemindAt");

                snooze.Property(s => s.SnoozeDurationMinutes)
                    .HasColumnName("SnoozeDurationMinutes");

                snooze.Property(s => s.SnoozeCount)
                    .HasColumnName("SnoozeCount");

                snooze.Property(s => s.SnoozedAt)
                    .HasColumnName("SnoozedAt");
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.RemindAt);
            builder.HasIndex(x => new { x.RefEntityType, x.RefEntityId });
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.OwnerId, x.IsSent });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
