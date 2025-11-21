using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for PriorityScore entity
    /// </summary>
    public class PriorityScoreConfiguration : IEntityTypeConfiguration<PriorityScore>
    {
        public void Configure(EntityTypeBuilder<PriorityScore> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "PriorityScores", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FeatureId)
                .IsRequired();

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CalculatedScore)
                .IsRequired()
                .HasPrecision(8, 2);

            builder.Property(x => x.CalculatedAt)
                .IsRequired();

            builder.Property(x => x.CalculatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Configure PriorityInput value object as owned type
            builder.OwnsOne(x => x.Input, inputBuilder =>
            {
                inputBuilder.Property(i => i.Impact)
                    .HasColumnName("InputImpact")
                    .IsRequired();

                inputBuilder.Property(i => i.Effort)
                    .HasColumnName("InputEffort")
                    .IsRequired();

                inputBuilder.Property(i => i.Confidence)
                    .HasColumnName("InputConfidence")
                    .IsRequired();

                inputBuilder.Property(i => i.Rationale)
                    .HasColumnName("InputRationale")
                    .HasMaxLength(1000);
            });

            // Indexes
            builder.HasIndex(x => x.FeatureId);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.CalculatedScore);
            builder.HasIndex(x => x.CalculatedAt);
        }
    }
}
