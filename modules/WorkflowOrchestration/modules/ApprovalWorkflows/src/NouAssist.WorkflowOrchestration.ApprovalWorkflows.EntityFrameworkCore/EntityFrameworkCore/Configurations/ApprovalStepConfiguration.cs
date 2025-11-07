using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore.Configurations
{
    public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ApprovalStep> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                ApprovalDbProperties.DbTablePrefix + "ApprovalSteps",
                ApprovalDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.WorkflowId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.OrderIndex).IsRequired();

            builder.Property(x => x.Requirement)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.TimeoutAction)
                .HasMaxLength(100);

            // Collection properties as JSON
            builder.Property("_approverUserIds")
                .HasColumnName("ApproverUserIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property("_approverRoleIds")
                .HasColumnName("ApproverRoleIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property("_approverTeamIds")
                .HasColumnName("ApproverTeamIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property(x => x.Conditions)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

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

            // Indexes
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.OrderIndex);
        }
    }
}
