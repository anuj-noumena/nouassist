using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class ExperimentHypothesisConfiguration : IEntityTypeConfiguration<ExperimentHypothesis>
    {
        public void Configure(EntityTypeBuilder<ExperimentHypothesis> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "ExperimentHypotheses", 
                GrowthExperimentationDbProperties.DbSchema);

            // Composite key
            builder.HasKey(x => new { x.ExperimentId, x.Statement });

            builder.Property(x => x.Statement)
                .IsRequired()
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxHypothesisTextLength);

            builder.Property(x => x.Rationale)
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength);

            builder.Property(x => x.IsSupported);

            // Index for searching
            builder.HasIndex(x => x.ExperimentId);
        }
    }
}
