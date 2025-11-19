using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Entities;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Configurations
{
    public class WeeklyAvailabilitySlotConfiguration : IEntityTypeConfiguration<WeeklyAvailabilitySlot>
    {
        public void Configure(EntityTypeBuilder<WeeklyAvailabilitySlot> builder)
        {
            builder.ToTable(TimeManagementDbProperties.DbTablePrefix + "WeeklyAvailabilitySlots", TimeManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserAvailabilityId)
                .IsRequired();

            builder.Property(x => x.DayOfWeek)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasMaxLength(10);

            builder.Property(x => x.EndTime)
                .HasMaxLength(10);

            builder.Property(x => x.IsAvailable)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => new { x.UserAvailabilityId, x.DayOfWeek }).IsUnique();
        }
    }
}
