using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class DataModelReferenceConfiguration : IEntityTypeConfiguration<DataModelReference>
    {
        public void Configure(EntityTypeBuilder<DataModelReference> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "DataModelReferences",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure DataFieldSpec value object
            builder.OwnsOne(x => x.FieldSpecification, fieldBuilder =>
            {
                fieldBuilder.Property(f => f.FieldName)
                    .HasColumnName("FieldName")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.DataRequirement.MaxFieldNameLength);

                fieldBuilder.Property(f => f.DataType)
                    .HasColumnName("DataType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                fieldBuilder.Property(f => f.IsRequired)
                    .HasColumnName("IsRequired")
                    .IsRequired();

                fieldBuilder.Property(f => f.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(RequirementSpecificationManagementConsts.DataRequirement.MaxDescriptionLength);

                fieldBuilder.Property(f => f.MaxLength)
                    .HasColumnName("MaxLength");
            });

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_DataModelReferences_RequirementId");
        }
    }
}
