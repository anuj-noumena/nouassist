using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval.Configurations
{
    public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ApprovalStep> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                WorkManagementDbProperties.DbTablePrefix + "ApprovalSteps",
                WorkManagementDbProperties.DbSchema
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

            // **FIX 1:** Explicitly provide typeof(List<Guid>) to Serialize
            builder.Property<List<Guid>>("_approverUserIds") // <-- Assumed you mean Property<T> for a private field
                .HasColumnName("ApproverUserIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, typeof(List<Guid>), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // **FIX 2:** Explicitly provide typeof(List<Guid>) to Serialize
            builder.Property<List<Guid>>("_approverRoleIds") // <-- Assumed you mean Property<T> for a private field
                .HasColumnName("ApproverRoleIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, typeof(List<Guid>), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // **FIX 3:** Explicitly provide typeof(List<Guid>) to Serialize
            builder.Property<List<Guid>>("_approverTeamIds") // <-- Assumed you mean Property<T> for a private field
                .HasColumnName("ApproverTeamIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, typeof(List<Guid>), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // **FIX 4:** Explicitly provide typeof(Dictionary<string, object>) to Serialize
            builder.Property(x => x.Conditions)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, typeof(Dictionary<string, object>), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // **FIX 5:** Explicitly provide typeof(Dictionary<string, object>) for v.GetValues()
            builder.Property(x => x.CustomFields)
                .HasColumnName("CustomFields")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v.GetValues(),
                        typeof(Dictionary<string, object>), // <-- Explicit type
                        (System.Text.Json.JsonSerializerOptions)null),
                    v => CustomFieldValues.Create(
                        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                            v,
                            (System.Text.Json.JsonSerializerOptions)null))
                )
                .IsRequired();

            // **FIX 6:** Explicitly provide typeof(Dictionary<string, object>) to Serialize
            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, typeof(Dictionary<string, object>), (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // Indexes
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.OrderIndex);
        }
    }
}