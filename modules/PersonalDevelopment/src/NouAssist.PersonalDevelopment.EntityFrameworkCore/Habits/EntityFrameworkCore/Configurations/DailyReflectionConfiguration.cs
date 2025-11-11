using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Configurations
{
    public class DailyReflectionConfiguration : IEntityTypeConfiguration<DailyReflection>
    {
        public void Configure(EntityTypeBuilder<DailyReflection> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "DailyReflections",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.Rating);

            builder.Property(x => x.WentWell)
                .HasMaxLength(2000);

            builder.Property(x => x.ToImprove)
                .HasMaxLength(2000);

            builder.Property(x => x.Learned)
                .HasMaxLength(2000);

            builder.Property(x => x.Gratitude)
                .HasMaxLength(1000);

            builder.Property(x => x.TomorrowPriorities)
                .HasMaxLength(2000);

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.Rating);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
