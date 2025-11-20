using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Entities;

namespace NouAssist.TimeManagement.EntityFrameworkCore.Calendar.EntityFrameworkCore.Configurations
{
    public class CalendarEventExceptionConfiguration : IEntityTypeConfiguration<CalendarEventException>
    {
        public void Configure(EntityTypeBuilder<CalendarEventException> builder)
        {
            builder.ToTable(TimeManagementDbProperties.DbTablePrefix + "CalendarEventExceptions", TimeManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventId)
                .IsRequired();

            builder.Property(x => x.InstanceDate)
                .IsRequired();

            builder.Property(x => x.OriginalStart);
            builder.Property(x => x.OriginalEnd);
            builder.Property(x => x.ModifiedStart);
            builder.Property(x => x.ModifiedEnd);

            builder.Property(x => x.ModifiedTitle)
                .HasMaxLength(200);

            builder.Property(x => x.ModifiedDescription)
                .HasMaxLength(2000);

            builder.Property(x => x.ModifiedLocation)
                .HasMaxLength(500);

            builder.Property(x => x.ModifiedNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.IsCancelled)
                .IsRequired();

            builder.Property(x => x.CancelledAt);

            builder.Property(x => x.CancelledBy)
                .HasMaxLength(256);

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(1000);

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(256);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => new { x.EventId, x.InstanceDate });
            builder.HasIndex(x => x.InstanceDate);
        }
    }
}
