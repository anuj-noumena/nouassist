using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Search.Aggregates;
using NouAssist.Infrastructure.Search.Enums; // Add this for SearchableEntityType
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Linq; // Add this for LINQ operations
using System; // Add this for Enum.Parse

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Configurations
{
    public class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistory>
    {
        public void Configure(EntityTypeBuilder<SearchHistory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "SearchHistories",
                InfrastructureDbProperties.DbSchema);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Query)
                .IsRequired()
                .HasMaxLength(SearchHistory.MaxQueryLength);
            builder.Property(x => x.ResultCount);
            
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

            builder.Property(x => x.SearchedAt).IsRequired();
            builder.Property(x => x.TenantId);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CreationTime);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
