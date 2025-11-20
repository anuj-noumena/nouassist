using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class FunnelStageConfiguration : IEntityTypeConfiguration<FunnelStage>
    {
        public void Configure(EntityTypeBuilder<FunnelStage> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "FunnelStages", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FunnelId).IsRequired();
            builder.Property(x => x.IsExitStage);

            // Value Object Mapping - StageDefinition
            builder.OwnsOne(x => x.Definition, defBuilder => 
            {
                defBuilder.Property(d => d.Name)
                    .HasColumnName("StageName")
                    .HasMaxLength(GrowthExperimentationConstants.ConversionFunnels.MaxStageNameLength)
                    .IsRequired();
                    
                defBuilder.Property(d => d.Description)
                    .HasColumnName("StageDescription")
                    .HasMaxLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelDescriptionLength);
                    
                defBuilder.Property(d => d.Order)
                    .HasColumnName("StageOrder")
                    .IsRequired();
                    
                defBuilder.Property(d => d.Criteria)
                    .HasColumnName("StageCriteria")
                    .HasMaxLength(500);
            });

            // Value Object Mapping - ConversionRatePerStage
            builder.OwnsOne(x => x.ConversionRate, rateBuilder => 
            {
                rateBuilder.Property(r => r.StageId)
                    .HasColumnName("ConversionRateStageId");
                    
                rateBuilder.Property(r => r.EnteredCount)
                    .HasColumnName("EnteredCount");
                    
                rateBuilder.Property(r => r.CompletedCount)
                    .HasColumnName("CompletedCount");
                    
                rateBuilder.Property(r => r.MeasuredAt)
                    .HasColumnName("ConversionRateMeasuredAt");
            });

            // Indexing
            builder.HasIndex(x => x.FunnelId);
            builder.HasIndex(x => new { x.FunnelId, x.IsExitStage });
        }
    }
}
