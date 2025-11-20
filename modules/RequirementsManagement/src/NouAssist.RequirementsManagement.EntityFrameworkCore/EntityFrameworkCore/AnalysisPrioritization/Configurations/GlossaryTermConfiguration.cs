using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class GlossaryTermConfiguration : IEntityTypeConfiguration<GlossaryTerm>
    {
        public void Configure(EntityTypeBuilder<GlossaryTerm> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "GlossaryTerms",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalysisConsts.MaxGlossaryTermNameLength)
                .HasColumnName(nameof(GlossaryTerm.Name));

            builder.Property(x => x.Definition)
                .IsRequired()
                .HasMaxLength(AnalysisConsts.MaxGlossaryTermDefinitionLength)
                .HasColumnName(nameof(GlossaryTerm.Definition));

            builder.Property(x => x.Context)
                .HasMaxLength(500)
                .HasColumnName(nameof(GlossaryTerm.Context));

            builder.Property(x => x.Examples)
                .HasMaxLength(1000)
                .HasColumnName(nameof(GlossaryTerm.Examples));

            builder.Property(x => x.RelatedTerms)
                .HasMaxLength(2000)
                .HasColumnName(nameof(GlossaryTerm.RelatedTerms));

            builder.Property(x => x.Synonyms)
                .HasMaxLength(500)
                .HasColumnName(nameof(GlossaryTerm.Synonyms));

            builder.Property(x => x.Acronyms)
                .HasMaxLength(200)
                .HasColumnName(nameof(GlossaryTerm.Acronyms));

            builder.Property(x => x.IsApproved)
                .IsRequired()
                .HasColumnName(nameof(GlossaryTerm.IsApproved));

            builder.Property(x => x.ApprovedBy)
                .HasColumnName(nameof(GlossaryTerm.ApprovedBy));

            builder.Property(x => x.ApprovedAt)
                .HasColumnName(nameof(GlossaryTerm.ApprovedAt));

            // Indexes
            builder.HasIndex(x => x.Name)
                .IsUnique()
                .HasDatabaseName("IX_GlossaryTerms_Name");

            builder.HasIndex(x => x.IsApproved)
                .HasDatabaseName("IX_GlossaryTerms_IsApproved");

            builder.HasIndex(x => x.ApprovedAt)
                .HasDatabaseName("IX_GlossaryTerms_ApprovedAt");

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_GlossaryTerms_TenantId");

            // Extra Properties and Concurrency
            builder.ConfigureExtraProperties();
            builder.ConfigureConcurrencyStamp();
        }
    }
}
