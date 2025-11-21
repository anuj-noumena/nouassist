using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Constants;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore.Configurations
{
    public class DocumentAggregateConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(DocumentManagementDbProperties.DbTablePrefix + "Documents", 
                           DocumentManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mappings

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

            // FileVersionNumber
            builder.OwnsOne(x => x.CurrentVersionNumber, versionBuilder =>
            {
                versionBuilder.Property(v => v.Major)
                    .IsRequired()
                    .HasColumnName("VersionMajor");

                versionBuilder.Property(v => v.Minor)
                    .IsRequired()
                    .HasColumnName("VersionMinor");
            });

            // AccessGrants Collection
            builder.OwnsMany(x => x.AccessGrants, accessBuilder =>
            {
                accessBuilder.ToTable(DocumentManagementDbProperties.DbTablePrefix + "DocumentAccessGrants",
                    DocumentManagementDbProperties.DbSchema);

                accessBuilder.WithOwner().HasForeignKey("DocumentId");
                accessBuilder.HasKey("DocumentId", "PrincipalId", "IsRole");

                accessBuilder.Property<Guid>("DocumentId").IsRequired();
                
                accessBuilder.Property(a => a.PrincipalId)
                    .IsRequired();

                accessBuilder.Property(a => a.IsRole)
                    .IsRequired();

                accessBuilder.Property(a => a.PermissionLevel)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                accessBuilder.Property(a => a.GrantedBy)
                    .IsRequired();

                accessBuilder.Property(a => a.GrantedAt)
                    .IsRequired();

                accessBuilder.HasIndex("DocumentId", "PrincipalId", "IsRole")
                    .IsUnique();
            });

            // Relationship with DocumentVersion (One-to-Many)
            builder.HasMany(x => x.Versions)
                .WithOne()
                .HasForeignKey(v => v.DocumentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex("Metadata_UploadedByUserId");
            builder.HasIndex(x => x.Status);
        }
    }
}
