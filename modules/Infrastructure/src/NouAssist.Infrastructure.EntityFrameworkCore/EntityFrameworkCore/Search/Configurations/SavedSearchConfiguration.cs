using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Search.Configurations
{
    public class SavedSearchConfiguration : IEntityTypeConfiguration<SavedSearch>
    {
        public void Configure(EntityTypeBuilder<SavedSearch> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "SavedSearches",
                InfrastructureDbProperties.DbSchema);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(SavedSearch.MaxNameLength);
            builder.Property(x => x.Query)
                .IsRequired()
                .HasMaxLength(SavedSearch.MaxQueryLength);
            builder.Property(x => x.Filters)
                .HasColumnType("jsonb"); // Store as JSONB
            builder.Property(x => x.UsageCount).IsRequired();
            builder.Property(x => x.LastUsedAt);
            builder.Property(x => x.TenantId);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CreationTime);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
