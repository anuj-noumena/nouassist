using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class AdviceRecordConfiguration : IEntityTypeConfiguration<AdviceRecord>
    {
        public void Configure(EntityTypeBuilder<AdviceRecord> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "AdviceRecords",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.MentorProfileId, x.Topic, x.GivenOn });

            builder.Property(x => x.Topic)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Title);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);

            builder.Property(x => x.GivenOn)
                .IsRequired();

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            builder.HasIndex(x => x.MentorProfileId);
            builder.HasIndex(x => x.Topic);
            builder.HasIndex(x => x.GivenOn);
        }
    }
}
