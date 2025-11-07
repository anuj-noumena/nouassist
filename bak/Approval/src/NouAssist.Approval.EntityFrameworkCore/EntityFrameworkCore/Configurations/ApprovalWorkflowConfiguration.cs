using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Approval.EntityFrameworkCore.Configurations
{
    public class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
    {
        public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                ApprovalDbProperties.DbTablePrefix + "ApprovalWorkflows",
                ApprovalDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.WorkflowType)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Strategy)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.TimeoutAction)
                .HasMaxLength(100);

            // JSON columns
            builder.Property(x => x.CustomFieldValues)
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

            // Relationships - Steps
            builder.HasMany(typeof(ApprovalStep).FullName)
                .WithOne()
                .HasForeignKey("WorkflowId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.WorkflowType);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
