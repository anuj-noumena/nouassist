using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class HabitTrackingEntryConfiguration : IEntityTypeConfiguration<HabitTrackingEntry>
    {
        public void Configure(EntityTypeBuilder<HabitTrackingEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "HabitTrackingEntries",
                           PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.HabitId)
                .IsRequired();

            builder.Property(x => x.LogDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.Completed)
                .IsRequired();

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.HabitId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.HabitId, x.LogDate })
                .IsUnique();
            builder.HasIndex(x => x.Completed);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
