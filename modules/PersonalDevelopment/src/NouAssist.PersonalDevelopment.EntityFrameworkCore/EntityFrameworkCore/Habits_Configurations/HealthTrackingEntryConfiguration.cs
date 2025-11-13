using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Configurations
{
    public class HealthTrackingEntryConfiguration : IEntityTypeConfiguration<HealthTrackingEntry>
    {
        public void Configure(EntityTypeBuilder<HealthTrackingEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "HealthTrackingEntries",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.SleepHours)
                .IsRequired();

            builder.Property(x => x.WaterCount)
                .IsRequired();

            builder.Property(x => x.PostureChecks)
                .IsRequired();

            builder.Property(x => x.BreaksTaken)
                .IsRequired();

            builder.Property(x => x.WaterLogs)
                .HasMaxLength(1000);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
