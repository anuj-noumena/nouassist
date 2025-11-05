using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Goals.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Goals.EntityFrameworkCore.Configurations
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(GoalsDbProperties.DbTablePrefix + "Achievements",
                            GoalsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(x => x.Icon)
                .HasMaxLength(50);

            builder.Property(x => x.BadgeImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Rarity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Points)
                .IsRequired();

            builder.Property(x => x.CriteriaJson)
                .HasColumnType("jsonb");

            builder.Property(x => x.IsSecret)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.IsActive);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
