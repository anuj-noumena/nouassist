using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class ExperimentResultDataConfiguration : IEntityTypeConfiguration<ExperimentResultData>
    {
        public void Configure(EntityTypeBuilder<ExperimentResultData> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "ExperimentResults", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.VariantId).IsRequired();
            builder.Property(x => x.MetricName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MetricType).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.IsPrimary);
            builder.Property(x => x.RecordedAt);

            // Value Object Mapping - ResultValue
            builder.OwnsOne(x => x.Value, valueBuilder => 
            {
                valueBuilder.Property(v => v.MetricType)
                    .HasColumnName("ResultMetricType")
                    .HasConversion<string>()
                    .HasMaxLength(50);
                    
                valueBuilder.Property(v => v.Value)
                    .HasColumnName("ResultValue");
                    
                valueBuilder.Property(v => v.SampleSize)
                    .HasColumnName("ResultSampleSize");
                    
                valueBuilder.Property(v => v.StandardDeviation)
                    .HasColumnName("ResultStandardDeviation");
                    
                valueBuilder.Property(v => v.MarginOfError)
                    .HasColumnName("ResultMarginOfError");
            });

            // Indexing
            builder.HasIndex(x => x.VariantId);
            builder.HasIndex(x => new { x.VariantId, x.MetricName }).IsUnique();
        }
    }
}
