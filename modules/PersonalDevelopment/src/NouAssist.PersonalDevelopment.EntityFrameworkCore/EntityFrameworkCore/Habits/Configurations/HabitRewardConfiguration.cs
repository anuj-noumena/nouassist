using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Entities;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class HabitRewardConfiguration : IEntityTypeConfiguration<HabitReward>
    {
        public void Configure(EntityTypeBuilder<HabitReward> builder)
        {
            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "HabitRewards",
                           PersonalDevelopmentDbProperties.DbSchema);

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
