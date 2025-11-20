using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class RelationshipLinkConfiguration : IEntityTypeConfiguration<RelationshipLink>
    {
        public void Configure(EntityTypeBuilder<RelationshipLink> builder)
        {
            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "RelationshipLinks",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SourceRequirementId)
                .IsRequired()
                .HasColumnName(nameof(RelationshipLink.SourceRequirementId));

            builder.Property(x => x.TargetRequirementId)
                .IsRequired()
                .HasColumnName(nameof(RelationshipLink.TargetRequirementId));

            builder.Property(x => x.RelationshipType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(RelationshipLink.RelationshipType));

            builder.Property(x => x.Description)
                .HasMaxLength(AnalysisConsts.MaxRelationshipDescriptionLength)
                .HasColumnName(nameof(RelationshipLink.Description));

            builder.Property(x => x.CreatedTime)
                .IsRequired()
                .HasColumnName(nameof(RelationshipLink.CreatedTime));

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasColumnName(nameof(RelationshipLink.CreatedBy));

            // Indexes
            builder.HasIndex(x => new { x.SourceRequirementId, x.TargetRequirementId, x.RelationshipType })
                .IsUnique()
                .HasDatabaseName("IX_RelationshipLinks_Source_Target_Type");

            builder.HasIndex(x => x.TargetRequirementId)
                .HasDatabaseName("IX_RelationshipLinks_TargetRequirementId");

            builder.HasIndex(x => x.RelationshipType)
                .HasDatabaseName("IX_RelationshipLinks_RelationshipType");

            builder.HasIndex(x => x.CreatedTime)
                .HasDatabaseName("IX_RelationshipLinks_CreatedTime");
        }
    }
}
