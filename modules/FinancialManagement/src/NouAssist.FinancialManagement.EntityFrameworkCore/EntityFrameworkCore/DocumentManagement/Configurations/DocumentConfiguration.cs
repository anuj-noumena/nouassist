using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.DocumentManagement.EntityFrameworkCore.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                DocumentManagementDbProperties.DbTablePrefix + "Documents",
                DocumentManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // FileMetadata Value Object
            builder.OwnsOne(x => x.Metadata, metadata =>
            {
                metadata.Property(m => m.FileName)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("FileName");

                metadata.Property(m => m.ContentType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("ContentType");

                metadata.Property(m => m.FileSize)
                    .IsRequired()
                    .HasColumnName("FileSize");

                metadata.Property(m => m.FileExtension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FileExtension");

                metadata.Property(m => m.FileHash)
                    .HasMaxLength(128)
                    .HasColumnName("FileHash");
            });

            // StorageLocation Value Object
            builder.OwnsOne(x => x.StorageLocation, storage =>
            {
                storage.Property(s => s.Container)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("StorageContainer");

                storage.Property(s => s.BlobPath)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasColumnName("BlobPath");

                storage.Property(s => s.FullPath)
                    .IsRequired()
                    .HasMaxLength(1280)
                    .HasColumnName("FullPath");

                storage.Property(s => s.StorageProvider)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("StorageProvider");
            });

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Document.Type));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Document.Status));

            builder.Property(x => x.AccessLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Document.AccessLevel));

            // EntityReference Value Object (nullable)
            builder.OwnsOne(x => x.EntityReference, entityRef =>
            {
                entityRef.Property(e => e.EntityType)
                    .HasMaxLength(256)
                    .HasColumnName("EntityType");

                entityRef.Property(e => e.EntityId)
                    .HasMaxLength(256)
                    .HasColumnName("EntityId");

                entityRef.Property(e => e.ReferenceDescription)
                    .HasMaxLength(512)
                    .HasColumnName("ReferenceDescription");
            });

            // OCRResult Value Object (nullable)
            builder.OwnsOne(x => x.OCRResult, ocr =>
            {
                ocr.Property(o => o.ExtractedText)
                    .HasColumnType("text")
                    .HasColumnName("OCRExtractedText");

                ocr.Property(o => o.ExtractedData)
                    .HasColumnName("OCRExtractedData")
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions)null));

                ocr.Property(o => o.ConfidenceScore)
                    .HasPrecision(5, 4)
                    .HasColumnName("OCRConfidenceScore");

                ocr.Property(o => o.ProcessedAt)
                    .HasColumnName("OCRProcessedAt");

                ocr.Property(o => o.ProcessorVersion)
                    .HasMaxLength(50)
                    .HasColumnName("OCRProcessorVersion");
            });

            builder.Property(x => x.Description)
                .HasMaxLength(2048)
                .HasColumnName(nameof(Document.Description));

            // Tags stored as JSON
            builder.Property(x => x.Tags)
                .HasColumnName(nameof(Document.Tags))
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions)null));

            builder.Property(x => x.ArchivedAt)
                .HasColumnName(nameof(Document.ArchivedAt));

            builder.Property(x => x.DownloadCount)
                .HasDefaultValue(0)
                .HasColumnName(nameof(Document.DownloadCount));

            builder.Property(x => x.LastAccessedAt)
                .HasColumnName(nameof(Document.LastAccessedAt));

            // Indexes
            builder.HasIndex("Metadata_FileHash")
                .HasDatabaseName("IX_Documents_FileHash");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_Documents_Type");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Documents_Status");

            builder.HasIndex("EntityReference_EntityType", "EntityReference_EntityId")
                .HasDatabaseName("IX_Documents_Entity");

            builder.HasIndex(x => x.CreationTime)
                .HasDatabaseName("IX_Documents_CreationTime");

            builder.HasIndex(x => new { x.Status, x.Type })
                .HasDatabaseName("IX_Documents_Status_Type");

            // Full-text search on description (PostgreSQL)
            builder.HasIndex(x => x.Description)
                .HasMethod("gin")
                .HasDatabaseName("IX_Documents_Description_FullText");
        }
    }
}
