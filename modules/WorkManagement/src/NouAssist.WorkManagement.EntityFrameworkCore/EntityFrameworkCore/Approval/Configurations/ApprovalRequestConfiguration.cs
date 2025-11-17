using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval.Configurations
{
    public class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
    {
        public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                WorkManagementDbProperties.DbTablePrefix + "ApprovalRequests",
                WorkManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.WorkflowId).IsRequired();
            builder.Property(x => x.EntityType).IsRequired().HasMaxLength(100);
            builder.Property(x => x.EntityId).IsRequired();
            builder.Property(x => x.RequestedBy).IsRequired();

            builder.Property(x => x.Title).HasMaxLength(500);
            builder.Property(x => x.Description).HasMaxLength(2000);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.CancellationReason).HasMaxLength(1000);

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            
            // Collections as JSON
            var stepIdsConverter = new ValueConverter<List<Guid>, string>(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<Guid>>(v, jsonOptions) ?? new List<Guid>()
            );

            var stepIdsComparer = new ValueComparer<List<Guid>>(
                (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList() // Snapshot method to prevent reference mutation issues
            );

            builder.Property<List<Guid>>("_completedStepIds") // Try to use generic property access if possible, or cast
                .HasColumnName("CompletedStepIds")
                .HasColumnType("jsonb")
                .HasConversion(stepIdsConverter, stepIdsComparer);
            
            
            // CustomFields as CustomFieldValues value object stored as JSONB
            var customFieldsConverter = new ValueConverter<CustomFieldValues, string>(
                v => JsonSerializer.Serialize(v.GetValues(), jsonOptions),
                v => CustomFieldValues.Create(JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
            );

            var customFieldsComparer = new ValueComparer<CustomFieldValues>(
                (c1, c2) => JsonSerializer.Serialize(c1.GetValues(), jsonOptions) == JsonSerializer.Serialize(c2.GetValues(), jsonOptions),
                c => c.GetValues().GetHashCode(),
                c => CustomFieldValues.Create(new Dictionary<string, object>(c.GetValues())) // Deep copy for snapshot
            );

            builder.Property(x => x.CustomFields)
                .HasColumnName("CustomFields")
                .HasColumnType("jsonb")
                .HasConversion(customFieldsConverter, customFieldsComparer)
                .IsRequired();
            
            // Metadata as Metadata value object stored as JSONB
            var metadataConverter = new ValueConverter<Metadata, string>(
                v => JsonSerializer.Serialize(v.GetValues(), jsonOptions),
                v => Metadata.Create(JsonSerializer.Deserialize<Dictionary<string, object>>(v, jsonOptions))
            );

            var metadataComparer = new ValueComparer<Metadata>(
                (c1, c2) => JsonSerializer.Serialize(c1.GetValues(), jsonOptions) == JsonSerializer.Serialize(c2.GetValues(), jsonOptions),
                c => c.GetValues().GetHashCode(),
                c => Metadata.Create(new Dictionary<string, object>(c.GetValues()))
            );

            builder.Property(x => x.Metadata)
                .HasColumnName("Metadata")
                .HasColumnType("jsonb")
                .HasConversion(metadataConverter, metadataComparer)
                .IsRequired();

            // SLA Deadline
            builder.Property(x => x.SlaDeadline);

            // Relationships - Decisions
            builder.HasMany(typeof(ApprovalDecision).FullName)
                .WithOne()
                .HasForeignKey("RequestId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.EntityType);
            builder.HasIndex(x => x.EntityId);
            builder.HasIndex(x => x.RequestedBy);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CurrentStepId);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.SlaDeadline);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
