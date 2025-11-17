using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class ResourceHistoryConfiguration : IEntityTypeConfiguration<ResourceHistory>
    {
        public void Configure(EntityTypeBuilder<ResourceHistory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                SkillDevelopmentDbProperties.DbTablePrefix + "ResourceHistories",
                SkillDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ResourceId)
                .IsRequired()
                .HasMaxLength(ResourceHistoryConsts.MaxResourceIdLength);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ResourceHistoryConsts.MaxTitleLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SourceUrl)
                .HasMaxLength(ResourceHistoryConsts.MaxSourceUrlLength);

            builder.Property(x => x.AccessedAt)
                .IsRequired();

            builder.Property(x => x.Progress)
                .IsRequired();

            // RelatedSkillIds - stored as JSON array
            builder.Property(x => x.RelatedSkillIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("RelatedSkillIds")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ResourceId);
            builder.HasIndex(x => new { x.UserId, x.Progress });
            builder.HasIndex(x => x.AccessedAt);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ResourceHistory.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
