using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Constants;
using NouAssist.RequirementsManagement.Traceability.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for TraceLink aggregate
    /// </summary>
    public class TraceLinkConfiguration : IEntityTypeConfiguration<TraceLink>
    {
        public void Configure(EntityTypeBuilder<TraceLink> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                TraceabilityManagementDbProperties.DbTablePrefix + "TraceLinks", 
                TraceabilityManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.LinkType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LinkDirection)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CustomLinkType)
                .HasMaxLength(TraceabilityManagementConsts.MaxCustomLinkTypeLength);

            builder.Property(x => x.Description)
                .HasMaxLength(TraceabilityManagementConsts.MaxLinkDescriptionLength);

            // Owner Shared Type configuration for LinkIdentifier value object
            builder.OwnsOne(x => x.Identifier, identifierBuilder =>
            {
                identifierBuilder.Property(i => i.Value)
                    .HasColumnName("LinkIdentifier")
                    .IsRequired()
                    .HasMaxLength(TraceabilityManagementConsts.MaxLinkIdentifierLength);
            });

            // Navigation properties
            builder.Navigation(x => x.SourceArtifact).IsRequired();
            builder.Navigation(x => x.TargetArtifact).IsRequired();

            // Indexing
            builder.HasIndex(x => x.Identifier);
            builder.HasIndex(x => x.IsSuspect);
            builder.HasIndex(x => new { x.LinkType, x.LinkDirection });
        }
    }
}
