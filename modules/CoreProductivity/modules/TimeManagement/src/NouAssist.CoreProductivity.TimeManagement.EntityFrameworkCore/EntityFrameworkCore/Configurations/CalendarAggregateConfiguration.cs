using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Configurations
{
    public class CalendarAggregateConfiguration : IEntityTypeConfiguration<Aggregates.Calendar>
    {
        public void Configure(EntityTypeBuilder<Aggregates.Calendar> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CalendarDbProperties.DbTablePrefix + "Calendars", CalendarDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.Timezone)
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDefault)
                .IsRequired();

            builder.Property(x => x.IsShared)
                .IsRequired();

            // Relationship with CalendarShare
            builder.HasMany(x => x.Shares)
                .WithOne()
                .HasForeignKey(s => s.CalendarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.IsDefault });
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
