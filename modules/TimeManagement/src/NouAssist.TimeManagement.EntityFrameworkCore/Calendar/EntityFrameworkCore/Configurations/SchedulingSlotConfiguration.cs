using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.TimeManagement.Calendar.Entities;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Configurations
{
    public class SchedulingSlotConfiguration : IEntityTypeConfiguration<SchedulingSlot>
    {
        public void Configure(EntityTypeBuilder<SchedulingSlot> builder)
        {
            builder.ToTable(TimeManagementDbProperties.DbTablePrefix + "SchedulingSlots", TimeManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequestId)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            builder.Property(x => x.VotesCount)
                .IsRequired();

            builder.Property(x => x.IsSelected)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Collection: AvailableParticipants (stored as JSON array)
            builder.Property(x => x.AvailableParticipants)
                .IsRequired();
            // Indexes
            builder.HasIndex(x => x.RequestId);
            builder.HasIndex(x => new { x.StartTime, x.EndTime });
        }
    }
}
