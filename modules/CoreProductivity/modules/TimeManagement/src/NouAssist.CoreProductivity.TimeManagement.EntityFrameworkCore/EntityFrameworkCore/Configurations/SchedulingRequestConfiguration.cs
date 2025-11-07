using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Configurations
{
    public class SchedulingRequestConfiguration : IEntityTypeConfiguration<SchedulingRequest>
    {
        public void Configure(EntityTypeBuilder<SchedulingRequest> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "SchedulingRequests", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequesterId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.DurationMinutes)
                .IsRequired();

            builder.Property(x => x.PreferredDate);

            builder.Property(x => x.PreferredTimeStart)
                .HasMaxLength(10);

            builder.Property(x => x.PreferredTimeEnd)
                .HasMaxLength(10);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SelectedSlotId);
            builder.Property(x => x.ScheduledAt);
            builder.Property(x => x.CalendarEventId);

            // Collection: ParticipantIds (stored as JSON array)
            builder.OwnsMany(x => x.ParticipantIds, pidBuilder =>
            {
                pidBuilder.ToJson();
            });

            // Relationship with SchedulingSlot
            builder.HasMany(x => x.Slots)
                .WithOne()
                .HasForeignKey(s => s.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.RequesterId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.RequesterId, x.Status });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
