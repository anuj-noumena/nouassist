using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Entities;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Configurations
{
    public class CalendarShareConfiguration : IEntityTypeConfiguration<CalendarShare>
    {
        public void Configure(EntityTypeBuilder<CalendarShare> builder)
        {
            builder.ToTable(CalendarDbProperties.DbTablePrefix + "CalendarShares", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CalendarId)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.SharedWithUserId)
                .IsRequired();

            builder.Property(x => x.PermissionLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.AcceptedAt);
            builder.Property(x => x.RevokedAt);

            builder.Property(x => x.RevokedBy)
                .HasMaxLength(256);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => new { x.CalendarId, x.SharedWithUserId });
            builder.HasIndex(x => x.SharedWithUserId);
        }
    }
}
