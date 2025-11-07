using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Approval.EntityFrameworkCore.Configurations
{
    public class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
    {
        public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                ApprovalDbProperties.DbTablePrefix + "ApprovalRequests",
                ApprovalDbProperties.DbSchema
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

            // Collections as JSON
            builder.Property("_completedStepIds")
                .HasColumnName("CompletedStepIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property(x => x.CustomFields)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

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
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
