using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class PipelineStageConfiguration : IEntityTypeConfiguration<PipelineStage>
    {
        public void Configure(EntityTypeBuilder<PipelineStage> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "PipelineStages", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.FundraisingRoundId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Order)
                .IsRequired();

            // Indexing
            builder.HasIndex(x => x.FundraisingRoundId);
            builder.HasIndex(x => x.Order);
            builder.HasIndex(x => new { x.FundraisingRoundId, x.Name }).IsUnique();
        }
    }
}
