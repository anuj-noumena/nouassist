using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class SkillTreeConfiguration : IEntityTypeConfiguration<SkillTree>
    {
        public void Configure(EntityTypeBuilder<SkillTree> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LearningDevelopmentDbProperties.DbTablePrefix + "SkillTrees",
                LearningDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.SkillId)
                .IsRequired();

            builder.Property(x => x.VisualizationUrl)
                .HasMaxLength(SkillTreeConsts.MaxVisualizationUrlLength);

            // Structure - stored as JSON (nodes and edges)
            builder.Property(x => x.Structure)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("Structure")
                .HasColumnType("jsonb")
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.SkillId).IsUnique();
            builder.HasIndex(x => x.UserId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(SkillTree.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
