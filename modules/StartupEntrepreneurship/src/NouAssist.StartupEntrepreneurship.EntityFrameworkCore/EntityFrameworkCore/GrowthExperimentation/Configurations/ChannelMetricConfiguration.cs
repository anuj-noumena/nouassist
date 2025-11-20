using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class ChannelMetricConfiguration : IEntityTypeConfiguration<ChannelMetric>
    {
        public void Configure(EntityTypeBuilder<ChannelMetric> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "ChannelMetrics", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ChannelId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.Value);
            builder.Property(x => x.TargetValue);
            builder.Property(x => x.PreviousValue);
            builder.Property(x => x.ChangePercentage);
            builder.Property(x => x.LastUpdated);

            // Indexing
            builder.HasIndex(x => x.ChannelId);
            builder.HasIndex(x => new { x.ChannelId, x.Name }).IsUnique();
        }
    }
}
