using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Constants;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class SpecificationSectionConfiguration : IEntityTypeConfiguration<SpecificationSection>
    {
        public void Configure(EntityTypeBuilder<SpecificationSection> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "SpecificationSections",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StructureId)
                .IsRequired();

            builder.Property(x => x.SectionName)
                .IsRequired()
                .HasMaxLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength);

            builder.Property(x => x.Content)
                .HasMaxLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionContentLength);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.ParentSectionId);

            // Configure self-referencing relationship
            builder.HasOne<SpecificationSection>()
                   .WithMany()
                   .HasForeignKey(x => x.ParentSectionId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes
            builder.HasIndex(x => x.StructureId)
                   .HasDatabaseName("IX_SpecificationSections_StructureId");

            builder.HasIndex(x => x.ParentSectionId)
                   .HasDatabaseName("IX_SpecificationSections_ParentSectionId");

            builder.HasIndex(x => new { x.StructureId, x.Order })
                   .HasDatabaseName("IX_SpecificationSections_StructureId_Order");
        }
    }
}
