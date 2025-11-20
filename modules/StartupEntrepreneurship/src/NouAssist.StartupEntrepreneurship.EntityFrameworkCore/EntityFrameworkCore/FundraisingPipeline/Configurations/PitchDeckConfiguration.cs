using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class PitchDeckConfiguration : IEntityTypeConfiguration<PitchDeck>
    {
        public void Configure(EntityTypeBuilder<PitchDeck> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "PitchDecks", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.PitchDeck.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(FundraisingPipelineConsts.PitchDeck.MaxDescriptionLength);

            builder.Property(x => x.FundraisingRoundId)
                .IsRequired(false);

            // Relationship Mappings
            builder.HasMany(x => x.Versions)
                .WithOne()
                .HasForeignKey(v => v.PitchDeckId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Presentations)
                .WithOne()
                .HasForeignKey(p => p.PitchDeckId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Feedbacks)
                .WithOne()
                .HasForeignKey(f => f.PitchDeckId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.FundraisingRoundId);
        }
    }
}
