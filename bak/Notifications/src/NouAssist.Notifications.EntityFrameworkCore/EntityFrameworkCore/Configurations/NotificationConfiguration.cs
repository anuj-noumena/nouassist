using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Notifications.Aggregates;
using NouAssist.Notifications.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Notifications.EntityFrameworkCore.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(NotificationsDbProperties.DbTablePrefix + "Notifications", 
                           NotificationsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.NotificationType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Message)
                .HasMaxLength(1000);

            builder.Property(x => x.EntityType)
                .HasMaxLength(256);

            builder.Property(x => x.ActionUrl)
                .HasMaxLength(2048);

            builder.Property(x => x.ActionLabel)
                .HasMaxLength(100);

            builder.Property(x => x.ActorName)
                .HasMaxLength(256);

            builder.Property(x => x.DeliveryStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // JSON columns
            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb");

            // Owned collection for delivery logs
            builder.OwnsMany(x => x.DeliveryLogs, dl =>
            {
                dl.ToTable(NotificationsDbProperties.DbTablePrefix + "NotificationDeliveryLogs");
                
                dl.Property(x => x.Channel)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                dl.Property(x => x.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                dl.Property(x => x.ErrorMessage)
                    .HasMaxLength(2000);
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.IsRead });
            builder.HasIndex(x => x.DeliveryStatus);
            builder.HasIndex(x => x.ExpiresAt);
            builder.HasIndex(x => x.CreationTime);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
