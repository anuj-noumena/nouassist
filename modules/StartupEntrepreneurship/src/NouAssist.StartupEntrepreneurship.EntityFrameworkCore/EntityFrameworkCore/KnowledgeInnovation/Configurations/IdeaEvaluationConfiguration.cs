using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class IdeaEvaluationConfiguration : IEntityTypeConfiguration<IdeaEvaluation>
    {
        public void Configure(EntityTypeBuilder<IdeaEvaluation> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "IdeaEvaluations",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.IdeaId, x.EvaluatorId });

            builder.Property(x => x.EvaluatorName)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Name);

            builder.Property(x => x.EvaluationDate)
                .IsRequired();

            builder.Property(x => x.Comments)
                .HasMaxLength(2000);

            builder.Property(x => x.IsApproved)
                .IsRequired();

            // Value Object Collection: EvaluationScore (stored as JSON)
            builder.Property(x => x.Scores)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<EvaluationScore>>(v, (JsonSerializerOptions)null) ?? new List<EvaluationScore>())
                .HasColumnName("Scores");

            builder.HasIndex(x => x.IdeaId);
            builder.HasIndex(x => x.EvaluatorId);
        }
    }
}
