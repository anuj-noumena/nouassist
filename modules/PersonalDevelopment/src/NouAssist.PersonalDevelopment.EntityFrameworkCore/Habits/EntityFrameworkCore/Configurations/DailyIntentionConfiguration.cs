using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Habits.EntityFrameworkCore.Configurations
{
    public class DailyIntentionConfiguration : IEntityTypeConfiguration<DailyIntention>
    {
        public void Configure(EntityTypeBuilder<DailyIntention> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "DailyIntentions",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            // Intentions as JSON
            builder.Property(x => x.Intentions)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<string>()
                )
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
