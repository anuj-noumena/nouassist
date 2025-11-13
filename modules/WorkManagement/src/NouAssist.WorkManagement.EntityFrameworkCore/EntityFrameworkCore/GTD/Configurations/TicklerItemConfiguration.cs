using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.GTD.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD.Configurations
{
    public class TicklerItemConfiguration : IEntityTypeConfiguration<TicklerItem>
    {
        public void Configure(EntityTypeBuilder<TicklerItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(GtdDbProperties.DbTablePrefix + "TicklerItems",
                            GtdDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.Property(x => x.TickleDate)
                .IsRequired();

            builder.Property(x => x.TickleTime);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SurfacedAt);
            builder.Property(x => x.ActionedAt);
            builder.Property(x => x.DismissedAt);
            builder.Property(x => x.SnoozeUntil);

            builder.Property(x => x.ActionType)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Complex properties for ImportanceScore and UrgencyScore
            builder.OwnsOne(x => x.Importance, ib =>
            {
                ib.Property(i => i.Value)
                    .HasColumnName("ImportanceScore")
                    .IsRequired()
                    .HasPrecision(3, 2);
            });

            builder.OwnsOne(x => x.Urgency, ub =>
            {
                ub.Property(u => u.Value)
                    .HasColumnName("UrgencyScore")
                    .IsRequired()
                    .HasPrecision(3, 2);
            });

            builder.Property(x => x.IsRecurring)
                .IsRequired();

            // Complex property for RecurrenceRule
            builder.OwnsOne(x => x.RecurrencePattern, rb =>
            {
                rb.Property(r => r.Frequency)
                    .HasColumnName("RecurrenceFrequency")
                    .HasConversion<string>()
                    .HasMaxLength(20);

                rb.Property(r => r.Interval)
                    .HasColumnName("RecurrenceInterval");

                rb.Property(r => r.Until)
                    .HasColumnName("RecurrenceUntil");

                rb.Property(r => r.Count)
                    .HasColumnName("RecurrenceCount");

                rb.Property(r => r.ByWeekDay)
                    .HasColumnName("RecurrenceByWeekDay")
                    .HasColumnType("jsonb");

                rb.Property(r => r.ByMonthDay)
                    .HasColumnName("RecurrenceByMonthDay")
                    .HasColumnType("jsonb");

                rb.Property(r => r.Description)
                    .HasColumnName("RecurrenceDescription")
                    .HasMaxLength(200);
            });

            builder.HasIndex(x => new { x.OwnerId, x.TickleDate });
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.SnoozeUntil);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
