using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Entities;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class AcceptanceCriterionConfiguration : IEntityTypeConfiguration<AcceptanceCriterion>
    {
        public void Configure(EntityTypeBuilder<AcceptanceCriterion> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "AcceptanceCriteria",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure CriterionStatement value object
            builder.OwnsOne(x => x.Statement, statementBuilder =>
            {
                statementBuilder.Property(s => s.Statement)
                    .HasColumnName("Statement")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxStatementLength);

                statementBuilder.Property(s => s.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxDescriptionLength);

                statementBuilder.Property(s => s.Type)
                    .HasColumnName("Type")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });

            builder.Property(x => x.Order)
                .IsRequired();

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_AcceptanceCriteria_RequirementId");

            builder.HasIndex(x => new { x.RequirementId, x.Order })
                   .HasDatabaseName("IX_AcceptanceCriteria_RequirementId_Order");
        }
    }
}
