using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class AssociatedConstraintConfiguration : IEntityTypeConfiguration<AssociatedConstraint>
    {
        public void Configure(EntityTypeBuilder<AssociatedConstraint> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "AssociatedConstraints",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure ConstraintStatement value object
            builder.OwnsOne(x => x.Constraint, constraintBuilder =>
            {
                constraintBuilder.Property(c => c.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.Constraint.MaxNameLength);

                constraintBuilder.Property(c => c.Statement)
                    .HasColumnName("Statement")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.Constraint.MaxStatementLength);

                constraintBuilder.Property(c => c.Type)
                    .HasColumnName("Type")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_AssociatedConstraints_RequirementId");
        }
    }
}
