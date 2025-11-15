using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Configurations
{
    public class SkillLogConfiguration : IEntityTypeConfiguration<SkillLog>
    {
        public void Configure(EntityTypeBuilder<SkillLog> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                SkillDevelopmentDbProperties.DbTablePrefix + "SkillLogs",
                SkillDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.SkillId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(SkillLogConsts.MaxContentLength);

            // Metadata - stored as JSON
            builder.Property(x => x.Metadata)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("Metadata")
                .HasColumnType("jsonb");

            // Tags - stored as JSON array
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("Tags")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.SkillId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => new { x.SkillId, x.Type });
            builder.HasIndex(x => x.CreationTime);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(SkillLog.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
