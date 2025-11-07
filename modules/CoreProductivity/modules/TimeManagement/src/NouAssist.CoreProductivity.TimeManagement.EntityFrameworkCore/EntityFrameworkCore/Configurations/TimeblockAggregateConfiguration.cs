using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Configurations
{
    public class TimeblockAggregateConfiguration : IEntityTypeConfiguration<Timeblock>
    {
        public void Configure(EntityTypeBuilder<Timeblock> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "Timeblocks", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.CalendarEventId)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.CompletionNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.BlockType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.StartAt)
                .IsRequired();

            builder.Property(x => x.EndAt)
                .IsRequired();

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            builder.Property(x => x.IsFlexible)
                .IsRequired();

            builder.Property(x => x.AllowOverlap)
                .IsRequired();

            builder.Property(x => x.CalendarId);

            // Relationship with TimeblockTask
            builder.HasMany(x => x.Tasks)
                .WithOne()
                .HasForeignKey(t => t.TimeblockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.CalendarEventId);
            builder.HasIndex(x => new { x.StartAt, x.EndAt });
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.OwnerId, x.Status });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
