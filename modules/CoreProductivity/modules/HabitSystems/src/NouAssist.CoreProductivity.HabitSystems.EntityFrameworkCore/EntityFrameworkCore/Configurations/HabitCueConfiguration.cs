using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.HabitSystems.Entities;
using NouAssist.CoreProductivity.HabitSystems.Enums;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Configurations
{
    public class HabitCueConfiguration : IEntityTypeConfiguration<HabitCue>
    {
        public void Configure(EntityTypeBuilder<HabitCue> builder)
        {
            builder.ToTable(HabitsDbProperties.DbTablePrefix + "HabitCues",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HabitId)
                .IsRequired();

            builder.Property(x => x.CueType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.CueValue)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.HabitId);
            builder.HasIndex(x => x.CueType);
        }
    }
}
