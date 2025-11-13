using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits.Entities;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Configurations
{
    public class HabitStrategyConfiguration : IEntityTypeConfiguration<HabitStrategy>
    {
        public void Configure(EntityTypeBuilder<HabitStrategy> builder)
        {
            builder.ToTable(HabitsDbProperties.DbTablePrefix + "HabitStrategies",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HabitId)
                .IsRequired();

            builder.Property(x => x.StrategyType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.HabitId);
            builder.HasIndex(x => x.StrategyType);
        }
    }
}
