using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Habits.EntityFrameworkCore.Configurations
{
    public class DailyQuoteConfiguration : IEntityTypeConfiguration<DailyQuote>
    {
        public void Configure(EntityTypeBuilder<DailyQuote> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "DailyQuotes",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Author)
                .HasMaxLength(200);

            builder.Property(x => x.ViewedAt);

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date });
            builder.HasIndex(x => x.ViewedAt);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
