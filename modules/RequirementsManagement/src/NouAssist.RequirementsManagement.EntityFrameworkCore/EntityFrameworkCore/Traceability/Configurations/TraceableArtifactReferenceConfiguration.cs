using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Constants;
using NouAssist.RequirementsManagement.Traceability.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for TraceableArtifactReference entity
    /// </summary>
    public class TraceableArtifactReferenceConfiguration : IEntityTypeConfiguration<TraceableArtifactReference>
    {
        public void Configure(EntityTypeBuilder<TraceableArtifactReference> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                TraceabilityManagementDbProperties.DbTablePrefix + "ArtifactReferences", 
                TraceabilityManagementDbProperties.DbSchema);

            // Primary Key - composite key using TraceLinkId and artifact ID
            builder.HasKey(x => new { x.TraceLinkId, ArtifactId = x.ArtifactIdentifier.Id, ArtifactType = x.ArtifactIdentifier.Type });

            // Foreign Key
            builder.HasOne<TraceLink>()
                .WithMany()
                .HasForeignKey(x => x.TraceLinkId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(TraceabilityManagementConsts.MaxArtifactNameLength);

            // Value Object mapping - ArtifactIdentifier
            builder.OwnsOne(x => x.ArtifactIdentifier, artifactIdBuilder =>
            {
                artifactIdBuilder.Property(a => a.Id)
                    .HasColumnName("ArtifactId")
                    .IsRequired()
                    .HasMaxLength(TraceabilityManagementConsts.MaxArtifactIdLength);

                artifactIdBuilder.Property(a => a.Type)
                    .HasColumnName("ArtifactType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                artifactIdBuilder.Property(a => a.Version)
                    .HasColumnName("ArtifactVersion")
                    .HasMaxLength(TraceabilityManagementConsts.MaxArtifactVersionLength);

                artifactIdBuilder.Property(a => a.Uri)
                    .HasColumnName("ArtifactUri")
                    .HasMaxLength(TraceabilityManagementConsts.MaxArtifactUriLength);
            });

            // Indexing
            builder.HasIndex(x => new { ArtifactId = EF.Property<string>(x, "ArtifactId"), ArtifactType = EF.Property<string>(x, "ArtifactType") });
            builder.HasIndex(x => x.IsSuspect);
        }
    }
}
