using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class AssociatedRuleConfiguration : IEntityTypeConfiguration<AssociatedRule>
    {
        public void Configure(EntityTypeBuilder<AssociatedRule> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "AssociatedRules",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure RuleDescription value object
            builder.OwnsOne(x => x.Rule, ruleBuilder =>
            {
                ruleBuilder.Property(r => r.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.BusinessRule.MaxNameLength);

                ruleBuilder.Property(r => r.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.BusinessRule.MaxDescriptionLength);
            });

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_AssociatedRules_RequirementId");
        }
    }
}
