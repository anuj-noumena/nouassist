using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Reviews.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Reviews.EntityFrameworkCore.Configurations
{
    public class ReviewProgressConfiguration : IEntityTypeConfiguration<ReviewProgress>
    {
        public void Configure(EntityTypeBuilder<ReviewProgress> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ReviewsDbProperties.DbTablePrefix + "ReviewProgresses",
                           ReviewsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReviewId)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.Property(x => x.TotalItems)
                .IsRequired();

            builder.Property(x => x.ReviewedItems)
                .IsRequired();

            builder.Property(x => x.RemainingItems)
                .IsRequired();

            builder.Property(x => x.CompletionPercentage)
                .IsRequired();

            // Metadata as JSON
            builder.Property(x => x.Metadata)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, object>()
                )
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.ReviewId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.ReviewId, x.Timestamp });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
