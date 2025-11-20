using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class NegotiationNoteConfiguration : IEntityTypeConfiguration<NegotiationNote>
    {
        public void Configure(EntityTypeBuilder<NegotiationNote> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "NegotiationNotes", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.TermSheetId)
                .IsRequired();

            builder.Property(x => x.RelatedClauseId)
                .IsRequired(false);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(2000);

            // Indexing
            builder.HasIndex(x => x.TermSheetId);
            builder.HasIndex(x => x.RelatedClauseId);
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
