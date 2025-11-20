using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class ConversionFunnelDefinitionConfiguration : IEntityTypeConfiguration<ConversionFunnelDefinition>
    {
        public void Configure(EntityTypeBuilder<ConversionFunnelDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "ConversionFunnels", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelDescriptionLength);

            builder.Property(x => x.IsActive);

            // Relationship Mapping - Stages
            builder.HasMany(x => x.Stages)
                .WithOne()
                .HasForeignKey(s => s.FunnelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.IsActive);
        }
    }
}
