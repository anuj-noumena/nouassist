using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class RequirementAggregateConfiguration : IEntityTypeConfiguration<Requirement>
    {
        public void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "Requirements",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Configure RequirementIdentifier value object
            builder.OwnsOne(x => x.Identifier, identifierBuilder =>
            {
                identifierBuilder.Property(i => i.Value)
                    .HasColumnName("Identifier")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.Requirement.MaxIdentifierLength);
            });

            // Configure RequirementText value object
            builder.OwnsOne(x => x.Text, textBuilder =>
            {
                textBuilder.Property(t => t.Title)
                    .HasColumnName("Title")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.Requirement.MaxTitleLength);

                textBuilder.Property(t => t.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength);
            });

            // Configure enums
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Configure Version
            builder.Property(x => x.Version)
                .IsRequired()
                .HasMaxLength(RequirementSpecificationManagementConsts.Requirement.MaxVersionLength);

            // Configure relationships to child entities
            builder.HasMany<AcceptanceCriterion>("_acceptanceCriteria")
                   .WithOne()
                   .HasForeignKey(ac => ac.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<AssociatedRule>("_associatedRules")
                   .WithOne()
                   .HasForeignKey(ar => ar.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<AssociatedConstraint>("_associatedConstraints")
                   .WithOne()
                   .HasForeignKey(ac => ac.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<DataModelReference>("_dataModelReferences")
                   .WithOne()
                   .HasForeignKey(dmr => dmr.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<InterfaceSpecification>("_interfaceSpecifications")
                   .WithOne()
                   .HasForeignKey(ifs => ifs.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<BusinessObjectiveLink>("_businessObjectiveLinks")
                   .WithOne()
                   .HasForeignKey(bol => bol.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<RequirementScopeMarker>("_scopeMarker")
                   .WithOne()
                   .HasForeignKey<RequirementScopeMarker>(rsm => rsm.RequirementId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.HasIndex(x => x.Identifier.Value)
                   .IsUnique()
                   .HasDatabaseName("IX_Requirements_Identifier");

            builder.HasIndex(x => x.Type)
                   .HasDatabaseName("IX_Requirements_Type");

            builder.HasIndex(x => x.Status)
                   .HasDatabaseName("IX_Requirements_Status");

            builder.HasIndex(x => x.Priority)
                   .HasDatabaseName("IX_Requirements_Priority");

            // Configure concurrency stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

            // Configure extra properties (handled by convention)
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(Requirement.ExtraProperties));
        }
    }
}
