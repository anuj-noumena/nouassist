using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class BusinessObjectiveLinkConfiguration : IEntityTypeConfiguration<BusinessObjectiveLink>
    {
        public void Configure(EntityTypeBuilder<BusinessObjectiveLink> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "BusinessObjectiveLinks",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure ObjectiveId value object
            builder.OwnsOne(x => x.ObjectiveId, objectiveBuilder =>
            {
                objectiveBuilder.Property(o => o.Value)
                    .HasColumnName("ObjectiveId")
                    .IsRequired();
            });

            builder.Property(x => x.Justification)
                .HasMaxLength(1000);

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_BusinessObjectiveLinks_RequirementId");

            builder.HasIndex(x => x.ObjectiveId.Value)
                   .HasDatabaseName("IX_BusinessObjectiveLinks_ObjectiveId");

            // Unique constraint to prevent duplicate links
            builder.HasIndex(x => new { x.RequirementId, x.ObjectiveId.Value })
                   .IsUnique()
                   .HasDatabaseName("IX_BusinessObjectiveLinks_RequirementId_ObjectiveId");
        }
    }
}
