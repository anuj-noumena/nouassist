using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class ExperimentVariantConfiguration : IEntityTypeConfiguration<ExperimentVariant>
    {
        public void Configure(EntityTypeBuilder<ExperimentVariant> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "ExperimentVariants", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExperimentId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxVariantNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping - VariantSetup
            builder.OwnsOne(x => x.Setup, setupBuilder => 
            {
                setupBuilder.Property(s => s.TrafficAllocation)
                    .HasColumnName("TrafficAllocation");
                    
                setupBuilder.Property(s => s.Configuration)
                    .HasColumnName("Configuration")
                    .HasColumnType("jsonb");
                    
                setupBuilder.Property(s => s.Url)
                    .HasColumnName("VariantUrl")
                    .HasMaxLength(2000);
            });

            // Indexing
            builder.HasIndex(x => new { x.ExperimentId, x.Name }).IsUnique();
            builder.HasIndex(x => new { x.ExperimentId, x.Type });
        }
    }
}
