using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Configurations
{
    public class CalendarEventAggregateConfiguration : IEntityTypeConfiguration<CalendarEvent>
    {
        public void Configure(EntityTypeBuilder<CalendarEvent> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TimeManagementDbProperties.DbTablePrefix + "CalendarEvents", TimeManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CalendarId)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.Property(x => x.EventLocation)
                .HasMaxLength(500);

            builder.Property(x => x.StartAt)
                .IsRequired();

            builder.Property(x => x.EndAt)
                .IsRequired();

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            builder.Property(x => x.IsAllDay)
                .IsRequired();

            builder.Property(x => x.IsCancelled)
                .IsRequired();

            builder.Property(x => x.CancelledAt);

            builder.Property(x => x.CancelledBy)
                .HasMaxLength(256);

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(1000);

            builder.Property(x => x.IsRecurring)
                .IsRequired();

            builder.Property(x => x.AllowOverlap)
                .IsRequired();

            // Value Object: RecurrenceRule (stored as JSON)
            builder.OwnsOne(x => x.RecurrenceRule, rrBuilder =>
            {
                rrBuilder.ToJson();
                rrBuilder.Property(rr => rr.Frequency).IsRequired();
                rrBuilder.Property(rr => rr.Interval).IsRequired();
            });

            // Collection: LinkedEntities (stored as JSON)
            builder.OwnsMany(x => x.LinkedEntities, leBuilder =>
            {
                leBuilder.ToJson();
                leBuilder.Property(le => le.EntityId).IsRequired();
                leBuilder.Property(le => le.EntityType).IsRequired().HasConversion<string>();
            });

            // Relationship with CalendarEventException
            builder.HasMany(x => x.Exceptions)
                .WithOne()
                .HasForeignKey(e => e.EventId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.CalendarId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.StartAt, x.EndAt });
            builder.HasIndex(x => x.IsRecurring);
            builder.HasIndex(x => new { x.CalendarId, x.StartAt });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
