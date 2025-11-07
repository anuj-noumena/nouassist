using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Configurations
{
    public class EnergyTrackingEntryConfiguration : IEntityTypeConfiguration<EnergyTrackingEntry>
    {
        public void Configure(EntityTypeBuilder<EnergyTrackingEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "EnergyTrackingEntries",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.EnergyLevel)
                .IsRequired();

            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
