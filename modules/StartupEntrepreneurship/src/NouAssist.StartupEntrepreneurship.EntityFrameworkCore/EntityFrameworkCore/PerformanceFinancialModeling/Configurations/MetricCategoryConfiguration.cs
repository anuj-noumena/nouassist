using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Configurations
{
    public class MetricCategoryConfiguration : IEntityTypeConfiguration<MetricCategory>
    {
        public void Configure(EntityTypeBuilder<MetricCategory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PerformanceFinancialModelingDbProperties.DbTablePrefix + "MetricCategories", 
                           PerformanceFinancialModelingDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxCategoryNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(PerformanceFinancialModelingConsts.MaxCategoryDescriptionLength);

            builder.Property(x => x.Color)
                .HasMaxLength(7); // Hex color code

            builder.Property(x => x.DisplayOrder)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.DisplayOrder);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();

            // ExtraProperties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(MetricCategory.ExtraProperties));

            // ConcurrencyStamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
