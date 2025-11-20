using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class ResourceHistoryConfiguration : IEntityTypeConfiguration<ResourceHistory>
    {
        public void Configure(EntityTypeBuilder<ResourceHistory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LearningDevelopmentDbProperties.DbTablePrefix + "ResourceHistories",
                LearningDevelopmentDbProperties.DbSchema);

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
            var relatedSkillIdsConverter = new ValueConverter<List<Guid>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null));

            builder.Property("_relatedSkillIds")
                .HasColumnName("RelatedSkillIds")
                .HasColumnType("jsonb")
                .HasConversion(relatedSkillIdsConverter);

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
