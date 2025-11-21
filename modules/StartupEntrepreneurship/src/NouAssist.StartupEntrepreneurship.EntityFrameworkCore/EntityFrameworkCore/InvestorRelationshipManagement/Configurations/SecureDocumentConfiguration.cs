using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for SecureDocument entity
    /// </summary>
    public class SecureDocumentConfiguration : IEntityTypeConfiguration<SecureDocument>
    {
        public void Configure(EntityTypeBuilder<SecureDocument> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "SecureDocuments", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DataRoomId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(IRMConsts.DueDiligenceDataRoom.MaxDocumentNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.StoragePath)
                .IsRequired()
                .HasMaxLength(IRMConsts.DueDiligenceDataRoom.MaxDocumentPathLength);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Value Object Mapping - DocumentMetadata
            builder.OwnsOne(x => x.Metadata, metadataBuilder =>
            {
                metadataBuilder.Property(m => m.FileName)
                    .IsRequired()
                    .HasColumnName("OriginalFileName")
                    .HasMaxLength(500);

                metadataBuilder.Property(m => m.MimeType)
                    .IsRequired()
                    .HasColumnName("MimeType")
                    .HasMaxLength(200);

                metadataBuilder.Property(m => m.FileSize)
                    .IsRequired()
                    .HasColumnName("FileSize");

                metadataBuilder.Property(m => m.Hash)
                    .IsRequired()
                    .HasColumnName("FileHash")
                    .HasMaxLength(200);

                metadataBuilder.Property(m => m.UploadedAt)
                    .IsRequired()
                    .HasColumnName("UploadedAt");

                metadataBuilder.Property(m => m.UploadedBy)
                    .IsRequired()
                    .HasColumnName("UploadedBy");
            });

            // Store access logs as JSON
            builder.Property(x => x.AccessLogs)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v.Select(log => new {
                        UserId = log.UserId,
                        AccessedAt = log.AccessedAt,
                        Action = log.Action,
                        IpAddress = log.IpAddress,
                        UserAgent = log.UserAgent
                    }), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<IEnumerable<dynamic>>(v, (System.Text.Json.JsonSerializerOptions)null)?
                        .Select(log => new AccessLog(
                            Guid.Parse(log.GetProperty("UserId").GetString()),
                            DateTime.Parse(log.GetProperty("AccessedAt").GetString()),
                            log.GetProperty("Action").GetString(),
                            log.TryGetProperty("IpAddress", out var ip) ? ip.GetString() : null,
                            log.TryGetProperty("UserAgent", out var ua) ? ua.GetString() : null
                        )).ToList() ?? new List<AccessLog>()
                )
                .HasColumnName("AccessLogs")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.DataRoomId)
                .HasDatabaseName("IX_SecureDocuments_DataRoomId");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_SecureDocuments_Type");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_SecureDocuments_IsActive");

            builder.HasIndex("Metadata_UploadedAt")
                .HasDatabaseName("IX_SecureDocuments_UploadedAt");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(SecureDocument.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
