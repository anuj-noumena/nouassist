using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Calendar.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Calendar.EntityFrameworkCore.Configurations
{
    public class TimeblockTemplateConfiguration : IEntityTypeConfiguration<TimeblockTemplate>
    {
        public void Configure(EntityTypeBuilder<TimeblockTemplate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "TimeblockTemplates", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.BlockType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.DurationMinutes)
                .IsRequired();

            builder.Property(x => x.DefaultStartTime)
                .HasMaxLength(10);

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Collection: DaysOfWeek (stored as JSON array)
            builder.OwnsMany(x => x.DaysOfWeek, dowBuilder =>
            {
                dowBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
