using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class SpecificationStructureAggregateConfiguration : IEntityTypeConfiguration<SpecificationStructure>
    {
        public void Configure(EntityTypeBuilder<SpecificationStructure> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "SpecificationStructures",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength);

            builder.Property(x => x.DocumentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // Configure relationship to child entities
            builder.HasMany<SpecificationSection>("_sections")
                   .WithOne()
                   .HasForeignKey(ss => ss.StructureId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.HasIndex(x => x.Name)
                   .IsUnique()
                   .HasDatabaseName("IX_SpecificationStructures_Name");

            builder.HasIndex(x => x.DocumentType)
                   .HasDatabaseName("IX_SpecificationStructures_DocumentType");

            // Configure concurrency stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

            // Configure extra properties
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(SpecificationStructure.ExtraProperties));
        }
    }
}
