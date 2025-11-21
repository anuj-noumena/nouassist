using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for FeedbackData entity
    /// </summary>
    public class FeedbackDataConfiguration : IEntityTypeConfiguration<FeedbackData>
    {
        public void Configure(EntityTypeBuilder<FeedbackData> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "FeedbackData", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FeedbackSessionId)
                .IsRequired();

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Question)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Response)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            // Configure FeedbackRating value object as owned type
            builder.OwnsOne(x => x.Rating, ratingBuilder =>
            {
                ratingBuilder.Property(r => r.Value)
                    .HasColumnName("RatingValue")
                    .IsRequired();

                ratingBuilder.Property(r => r.MaxValue)
                    .HasColumnName("RatingMaxValue")
                    .IsRequired();

                ratingBuilder.Property(r => r.Category)
                    .HasColumnName("RatingCategory")
                    .HasMaxLength(50)
                    .IsRequired();

                ratingBuilder.Property(r => r.Comment)
                    .HasColumnName("RatingComment")
                    .HasMaxLength(500);
            });

            // Configure FeedbackTag collection as JSON
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(t => new { t.Name, t.Color, t.Category }), JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, JsonSerializerOptions.Default)!
                        .Select(item => new FeedbackTag(
                            item.GetProperty("Name").GetString()!,
                            item.GetProperty("Color").GetString()!,
                            item.GetProperty("Category").GetString()!))
                        .ToList())
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.FeedbackSessionId);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.RecordedAt);
        }
    }
}
