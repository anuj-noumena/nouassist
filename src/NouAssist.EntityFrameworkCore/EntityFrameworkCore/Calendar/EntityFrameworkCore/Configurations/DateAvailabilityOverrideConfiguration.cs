using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Calendar.Entities;

namespace NouAssist.Calendar.EntityFrameworkCore.Configurations
{
    public class DateAvailabilityOverrideConfiguration : IEntityTypeConfiguration<DateAvailabilityOverride>
    {
        public void Configure(EntityTypeBuilder<DateAvailabilityOverride> builder)
        {
            builder.ToTable(CalendarDbProperties.DbTablePrefix + "DateAvailabilityOverrides", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserAvailabilityId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.IsAvailable)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.Property(x => x.StartTime)
                .HasMaxLength(10);

            builder.Property(x => x.EndTime)
                .HasMaxLength(10);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => new { x.UserAvailabilityId, x.Date });
            builder.HasIndex(x => x.Date);
        }
    }
}
