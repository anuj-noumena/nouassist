using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.AuditTrail.Enums;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.AuditTrail.EntityFrameworkCore.Configurations
{
    public class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AuditTrailDbProperties.DbTablePrefix + "Entries",
                AuditTrailDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Timestamp
            builder.Property(x => x.Timestamp)
                .IsRequired()
                .HasColumnName(nameof(AuditEntry.Timestamp));

            // EntityReference Value Object
            builder.OwnsOne(x => x.EntityReference, entityRef =>
            {
                entityRef.Property(e => e.EntityType)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("EntityType");

                entityRef.Property(e => e.EntityId)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("EntityId");

                entityRef.Property(e => e.EntityName)
                    .HasMaxLength(512)
                    .HasColumnName("EntityName");
            });

            // Event Type
            builder.Property(x => x.EventType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(AuditEntry.EventType));

            // UserContext Value Object
            builder.OwnsOne(x => x.UserContext, userCtx =>
            {
                userCtx.Property(u => u.UserId)
                    .IsRequired()
                    .HasColumnName("UserId");

                userCtx.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("UserName");

                userCtx.Property(u => u.IPAddress)
                    .HasMaxLength(64)
                    .HasColumnName("IPAddress");

                userCtx.Property(u => u.UserAgent)
                    .HasMaxLength(512)
                    .HasColumnName("UserAgent");

                userCtx.Property(u => u.SessionId)
                    .HasMaxLength(256)
                    .HasColumnName("SessionId");
            });

            // Operation Description
            builder.Property(x => x.OperationDescription)
                .IsRequired()
                .HasMaxLength(1024)
                .HasColumnName(nameof(AuditEntry.OperationDescription));

            // Old/New Values (stored as JSON)
            builder.Property(x => x.OldValue)
                .HasMaxLength(4000)
                .HasColumnName(nameof(AuditEntry.OldValue));

            builder.Property(x => x.NewValue)
                .HasMaxLength(4000)
                .HasColumnName(nameof(AuditEntry.NewValue));

            // Metadata Value Object (stored as JSON)
            builder.OwnsOne(x => x.Metadata, metadata =>
            {
                metadata.Property(m => m.Properties)
                    .HasColumnName("Metadata")
                    .HasColumnType("jsonb") // PostgreSQL JSON type
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null));
            });

            // Severity
            builder.Property(x => x.Severity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(AuditEntry.Severity));

            // Compliance Flags (stored as integer for bitwise operations)
            builder.Property(x => x.ComplianceFlags)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName(nameof(AuditEntry.ComplianceFlags));

            // RetentionPolicy Value Object
            builder.OwnsOne(x => x.RetentionPolicy, retention =>
            {
                retention.Property(r => r.RetentionDays)
                    .IsRequired()
                    .HasColumnName("RetentionDays");

                retention.Property(r => r.ExpirationDate)
                    .IsRequired()
                    .HasColumnName("ExpirationDate");

                retention.Property(r => r.IsArchivable)
                    .IsRequired()
                    .HasColumnName("IsArchivable");

                retention.Property(r => r.ArchiveLocation)
                    .HasMaxLength(512)
                    .HasColumnName("ArchiveLocation");
            });

            // Archive info
            builder.Property(x => x.IsArchived)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(AuditEntry.IsArchived));

            builder.Property(x => x.ArchivedAt)
                .HasColumnName(nameof(AuditEntry.ArchivedAt));

            // Indexes for performance
            builder.HasIndex(x => x.Timestamp)
                .HasDatabaseName("IX_AuditEntries_Timestamp");

            builder.HasIndex(x => new { x.Timestamp, x.IsArchived })
                .HasDatabaseName("IX_AuditEntries_Timestamp_IsArchived");

            builder.HasIndex(x => x.Severity)
                .HasDatabaseName("IX_AuditEntries_Severity");

            builder.HasIndex(x => x.ComplianceFlags)
                .HasDatabaseName("IX_AuditEntries_ComplianceFlags");

            // Composite index for entity lookups
            builder.HasIndex("EntityReference_EntityType", "EntityReference_EntityId")
                .HasDatabaseName("IX_AuditEntries_Entity");

            // User lookups
            builder.HasIndex("UserContext_UserId")
                .HasDatabaseName("IX_AuditEntries_UserId");

            // Archival queries
            builder.HasIndex(x => new { x.IsArchived, x.ArchivedAt })
                .HasDatabaseName("IX_AuditEntries_Archive");

            // Partition by fiscal year for large datasets (optional)
            // This would require PostgreSQL table partitioning setup
        }
    }
}
