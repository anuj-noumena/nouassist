using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Notifications.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Notifications.Configurations
{
    public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
    {
        public void Configure(EntityTypeBuilder<NotificationPreference> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "NotificationPreferences", 
                InfrastructureDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.NotificationType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(x => x.CustomSchedule)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => new { x.UserId, x.NotificationType }).IsUnique();
            builder.HasIndex(x => x.TenantId);
        }
    }
}
