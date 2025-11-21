using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for Insight entity
    /// </summary>
    public class InsightConfiguration : IEntityTypeConfiguration<Insight>
    {
        public void Configure(EntityTypeBuilder<Insight> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "Insights", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.InterviewId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxInsightDescriptionLength);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Importance)
                .IsRequired();

            builder.Property(x => x.ExtractedAt)
                .IsRequired();

            builder.Property(x => x.ExtractedBy)
                .IsRequired()
                .HasMaxLength(100);

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
            builder.HasIndex(x => x.InterviewId);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.Importance);
            builder.HasIndex(x => x.ExtractedAt);
        }
    }
}
