using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Linq; // Add this for LINQ operations
using System; // Add this for Enum.Parse

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Configurations
{
    public class SearchAnalyticsConfiguration : IEntityTypeConfiguration<SearchAnalytics>
    {
        public void Configure(EntityTypeBuilder<SearchAnalytics> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "SearchAnalytics",
                InfrastructureDbProperties.DbSchema);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Query)
                .IsRequired()
                .HasMaxLength(SearchAnalytics.MaxQueryLength);
            builder.Property(x => x.ResultCount).IsRequired();
            builder.Property(x => x.SelectedResultId);
            builder.Property(x => x.SelectedResultPosition);
            builder.Property(x => x.SearchDurationMs);
            builder.Property(x => x.QualityScore).IsRequired();
            builder.Property(x => x.SessionId)
                .HasMaxLength(256); // Assuming a reasonable max length for session ID
            builder.Property(x => x.Timestamp).IsRequired();
            builder.Property(x => x.TenantId);

            // Storing enums as strings and dictionaries as JSONB
            builder.Property(x => x.EntityTypes)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => (SearchableEntityType)Enum.Parse(typeof(SearchableEntityType), s))
                          .ToList()
                )
                .HasColumnType("jsonb"); // Using jsonb for collections of enums

            builder.Property(x => x.Filters)
                .HasColumnType("jsonb"); // Store as JSONB

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.Query);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
