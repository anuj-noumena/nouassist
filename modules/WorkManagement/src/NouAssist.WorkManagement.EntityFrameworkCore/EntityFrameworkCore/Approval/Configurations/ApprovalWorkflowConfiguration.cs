using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval.Configurations
{
    public class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
    {
        public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                WorkManagementDbProperties.DbTablePrefix + "ApprovalWorkflows",
                WorkManagementDbProperties.DbSchema
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

            // CustomFields as CustomFieldValues value object stored as JSONB
            builder.Property(x => x.CustomFields)
                .HasColumnName("CustomFields")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v.GetValues(),
                        (System.Text.Json.JsonSerializerOptions)null),
                    v => CustomFieldValues.Create(
                        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                            v,
                            (System.Text.Json.JsonSerializerOptions)null))
                )
                .IsRequired();

            // Metadata as Dictionary<string, object> stored as JSONB
            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v,
                        (System.Text.Json.JsonSerializerOptions)null)
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