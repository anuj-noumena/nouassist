using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for SolutionPrototype aggregate
    /// </summary>
    public class SolutionPrototypeConfiguration : IEntityTypeConfiguration<SolutionPrototype>
    {
        public void Configure(EntityTypeBuilder<SolutionPrototype> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "SolutionPrototypes", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxPrototypeNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxPrototypeDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.RelatedHypothesisId)
                .IsRequired(false);

            builder.Property(x => x.Purpose)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.TargetAudience)
                .IsRequired()
                .HasMaxLength(500);

            // Configure child entity relationships
            builder.HasMany(x => x.Versions)
                .WithOne()
                .HasForeignKey("PrototypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to ProblemHypothesis
            builder.HasOne<ProblemHypothesis>()
                .WithMany()
                .HasForeignKey(x => x.RelatedHypothesisId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.RelatedHypothesisId);

            // Configure ExtraProperties and multi-tenancy
            builder.ConfigureExtraProperties();
            builder.ConfigureByConvention();
        }
    }
}
