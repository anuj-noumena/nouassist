using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Habits.Entities;

namespace NouAssist.Habits.EntityFrameworkCore.Configurations
{
    public class HabitRewardConfiguration : IEntityTypeConfiguration<HabitReward>
    {
        public void Configure(EntityTypeBuilder<HabitReward> builder)
        {
            builder.ToTable(HabitsDbProperties.DbTablePrefix + "HabitRewards",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HabitId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Craving)
                .HasMaxLength(500);

            builder.Property(x => x.RewardType)
                .HasMaxLength(100);

            builder.HasIndex(x => x.HabitId);
        }
    }
}
