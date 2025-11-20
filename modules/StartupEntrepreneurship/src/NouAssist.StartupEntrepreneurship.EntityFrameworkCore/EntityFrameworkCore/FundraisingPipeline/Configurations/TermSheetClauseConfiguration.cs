using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class TermSheetClauseConfiguration : IEntityTypeConfiguration<TermSheetClause>
    {
        public void Configure(EntityTypeBuilder<TermSheetClause> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "TermSheetClauses", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.TermSheetId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.TermSheet.MaxClauseNameLength);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Value Object Mapping - ClauseText
            builder.OwnsOne(x => x.Text, textBuilder =>
            {
                textBuilder.Property(t => t.Content)
                    .HasColumnName("TextContent")
                    .IsRequired()
                    .HasMaxLength(FundraisingPipelineConsts.TermSheet.MaxClauseValueLength);

                textBuilder.Property(t => t.IsNegotiable)
                    .HasColumnName("TextIsNegotiable")
                    .IsRequired();

                textBuilder.Property(t => t.Category)
                    .HasColumnName("TextCategory")
                    .IsRequired()
                    .HasMaxLength(100);

                textBuilder.Property(t => t.FormattedHtml)
                    .HasColumnName("TextFormattedHtml")
                    .HasMaxLength(4000);
            });

            // Indexing
            builder.HasIndex(x => x.TermSheetId);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.Order);
            builder.HasIndex(x => new { x.TermSheetId, x.Name }).IsUnique();
        }
    }
}
