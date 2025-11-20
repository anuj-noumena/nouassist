using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Goals;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Goals.Configurations
{
    public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
    {
        public void Configure(EntityTypeBuilder<UserAchievement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "UserAchievements",
                PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.AchievementId)
                .IsRequired();

            builder.Property(x => x.UnlockedAt)
                .IsRequired();

            builder.Property(x => x.ProgressPercentage)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(x => x.ProgressDataJson)
                .HasColumnType("jsonb");

            builder.Property(x => x.IsNotified)
                .IsRequired();

            builder.Property(x => x.NotifiedAt);

            builder.HasIndex(x => new { x.UserId, x.AchievementId }).IsUnique();
            builder.HasIndex(x => x.UserId);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
