using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class InterfaceSpecificationConfiguration : IEntityTypeConfiguration<InterfaceSpecification>
    {
        public void Configure(EntityTypeBuilder<InterfaceSpecification> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "InterfaceSpecifications",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Configure ApiSignature value object
            builder.OwnsOne(x => x.Signature, signatureBuilder =>
            {
                signatureBuilder.Property(s => s.InterfaceName)
                    .HasColumnName("InterfaceName")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxNameLength);

                signatureBuilder.Property(s => s.Endpoint)
                    .HasColumnName("Endpoint")
                    .IsRequired()
                    .HasMaxLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxEndpointLength);

                signatureBuilder.Property(s => s.HttpMethod)
                    .HasColumnName("HttpMethod")
                    .IsRequired()
                    .HasMaxLength(10);

                signatureBuilder.Property(s => s.Type)
                    .HasColumnName("Type")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                signatureBuilder.Property(s => s.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxDescriptionLength);
            });

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .HasDatabaseName("IX_InterfaceSpecifications_RequirementId");
        }
    }
}
