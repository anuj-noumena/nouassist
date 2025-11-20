using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Configurations
{
    public class PopularSearchConfiguration : IEntityTypeConfiguration<PopularSearch>
    {
        public void Configure(EntityTypeBuilder<PopularSearch> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "PopularSearches",
                InfrastructureDbProperties.DbSchema);

            builder.Property(x => x.Query)
                .IsRequired()
                .HasMaxLength(PopularSearch.MaxQueryLength);
            builder.Property(x => x.SearchCount).IsRequired();
            builder.Property(x => x.LastSearchedAt);
            builder.Property(x => x.TenantId);

            builder.HasIndex(x => x.Query).IsUnique(); // Query should be unique for popular searches
            builder.HasIndex(x => x.SearchCount);
            builder.HasIndex(x => x.CreationTime);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
