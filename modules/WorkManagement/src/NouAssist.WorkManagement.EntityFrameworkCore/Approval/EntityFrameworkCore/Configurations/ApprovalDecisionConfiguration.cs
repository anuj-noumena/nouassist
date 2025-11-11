using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Approval.Entities;

namespace NouAssist.Approval.EntityFrameworkCore.Configurations
{
    public class ApprovalDecisionConfiguration : IEntityTypeConfiguration<ApprovalDecision>
    {
        public void Configure(EntityTypeBuilder<ApprovalDecision> builder)
        {
            builder.ToTable(
                ApprovalDbProperties.DbTablePrefix + "ApprovalDecisions",
                ApprovalDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RequestId).IsRequired();
            builder.Property(x => x.StepId).IsRequired();
            builder.Property(x => x.ApproverId).IsRequired();

            builder.Property(x => x.Action)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Comments).HasMaxLength(2000);
            builder.Property(x => x.Signature).HasMaxLength(500);

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
            builder.HasIndex(x => x.RequestId);
            builder.HasIndex(x => x.StepId);
            builder.HasIndex(x => x.ApproverId);
            builder.HasIndex(x => x.Action);
            builder.HasIndex(x => x.DecidedAt);
        }
    }
}
