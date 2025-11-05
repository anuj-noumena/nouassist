using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Calendar.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Calendar.EntityFrameworkCore.Configurations
{
    public class UserAvailabilityConfiguration : IEntityTypeConfiguration<UserAvailability>
    {
        public void Configure(EntityTypeBuilder<UserAvailability> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "UserAvailabilities", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            // Relationship with WeeklyAvailabilitySlot
            builder.HasMany(x => x.WeeklySlots)
                .WithOne()
                .HasForeignKey(s => s.UserAvailabilityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with DateAvailabilityOverride
            builder.HasMany(x => x.DateOverrides)
                .WithOne()
                .HasForeignKey(o => o.UserAvailabilityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId).IsUnique();

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
