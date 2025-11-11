using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IdentityAccess.Users.EntityFrameworkCore.Configurations
{
    public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(UsersDbProperties.DbTablePrefix + "UserSettings",
                            UsersDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Theme)
                .HasMaxLength(50);

            builder.Property(x => x.Language)
                .HasMaxLength(10);

            builder.Property(x => x.Timezone)
                .HasMaxLength(50);

            builder.Property(x => x.DateFormat)
                .HasMaxLength(20);

            builder.Property(x => x.TimeFormat)
                .HasMaxLength(20);

            builder.Property(x => x.WeekStartDay)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.DefaultPomodoroMinutes)
                .IsRequired();

            builder.Property(x => x.DefaultShortBreakMinutes)
                .IsRequired();

            builder.Property(x => x.DefaultLongBreakMinutes)
                .IsRequired();

            builder.Property(x => x.EnableNotifications)
                .IsRequired();

            builder.Property(x => x.EnableSounds)
                .IsRequired();

            builder.Property(x => x.EnableDarkMode)
                .IsRequired();

            builder.Property(x => x.CustomSettingsJson)
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.UserId).IsUnique();

            builder.ApplyObjectExtensionMappings();
        }
    }
}
