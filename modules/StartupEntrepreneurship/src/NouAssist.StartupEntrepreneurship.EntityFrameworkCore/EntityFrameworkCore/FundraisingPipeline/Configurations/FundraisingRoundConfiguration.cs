using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class FundraisingRoundConfiguration : IEntityTypeConfiguration<FundraisingRound>
    {
        public void Configure(EntityTypeBuilder<FundraisingRound> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "FundraisingRounds", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.FundraisingRound.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(FundraisingPipelineConsts.FundraisingRound.MaxDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Stage)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TargetAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CommittedAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.TargetCloseDate)
                .IsRequired(false);

            builder.Property(x => x.ActualCloseDate)
                .IsRequired(false);

            // Relationship Mapping (One-to-Many for pipeline stages)
            builder.HasMany(x => x.PipelineStages)
                .WithOne()
                .HasForeignKey(s => s.FundraisingRoundId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.Stage);
            builder.HasIndex(x => x.StartDate);
        }
    }
}
