using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.DocumentManagement.Constants;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore.Configurations
{
    public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
    {
        public void Configure(EntityTypeBuilder<DocumentVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(DocumentManagementDbProperties.DbTablePrefix + "DocumentVersions", 
                           DocumentManagementDbProperties.DbSchema);

            // Composite Key
            builder.HasKey(x => new { x.DocumentId, VersionMajor = x.VersionNumber.Major, VersionMinor = x.VersionNumber.Minor });

            // FileVersionNumber
            builder.OwnsOne(x => x.VersionNumber, versionBuilder =>
            {
                versionBuilder.Property(v => v.Major)
                    .IsRequired()
                    .HasColumnName("VersionMajor");

                versionBuilder.Property(v => v.Minor)
                    .IsRequired()
                    .HasColumnName("VersionMinor");
            });

            // DocumentMetadata
            builder.OwnsOne(x => x.Metadata, metadataBuilder =>
            {
                metadataBuilder.Property(m => m.Filename)
                    .IsRequired()
                    .HasColumnName("Filename")
                    .HasMaxLength(DocumentManagementConsts.Document.MaxFilenameLength);

                metadataBuilder.Property(m => m.MimeType)
                    .IsRequired()
                    .HasColumnName("MimeType")
                    .HasMaxLength(DocumentManagementConsts.Document.MaxMimeTypeLength);

                metadataBuilder.Property(m => m.FileSize)
                    .IsRequired()
                    .HasColumnName("FileSize");

                metadataBuilder.Property(m => m.UploadedByUserId)
                    .IsRequired()
                    .HasColumnName("UploadedByUserId");

                metadataBuilder.Property(m => m.UploadedAt)
                    .IsRequired()
                    .HasColumnName("UploadedAt");
            });

            // StorageReference
            builder.OwnsOne(x => x.StorageReference, storageBuilder =>
            {
                storageBuilder.Property(s => s.StorageProvider)
                    .IsRequired()
                    .HasColumnName("StorageProvider")
                    .HasMaxLength(100);

                storageBuilder.Property(s => s.ContainerName)
                    .IsRequired()
                    .HasColumnName("ContainerName")
                    .HasMaxLength(200);

                storageBuilder.Property(s => s.BlobPath)
                    .IsRequired()
                    .HasColumnName("BlobPath")
                    .HasMaxLength(DocumentManagementConsts.Document.MaxPathLength);
            });

            // Version properties
            builder.Property(x => x.ChangeComment)
                .HasMaxLength(500);

            // Indexing
            builder.HasIndex(x => x.DocumentId);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex("Metadata_UploadedByUserId");
        }
    }
}
