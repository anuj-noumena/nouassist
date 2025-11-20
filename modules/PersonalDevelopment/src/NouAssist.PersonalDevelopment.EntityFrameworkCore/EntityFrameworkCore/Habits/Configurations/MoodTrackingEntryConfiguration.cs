using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class MoodTrackingEntryConfiguration : IEntityTypeConfiguration<MoodTrackingEntry>
    {
        public void Configure(EntityTypeBuilder<MoodTrackingEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "MoodTrackingEntries",
                           PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.MoodRating)
                .IsRequired();

            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            // Tags as JSON
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<string>()
                )
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.MoodRating);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
