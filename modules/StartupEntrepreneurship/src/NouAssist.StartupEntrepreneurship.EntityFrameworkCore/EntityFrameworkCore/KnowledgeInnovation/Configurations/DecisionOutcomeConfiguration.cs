using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class DecisionOutcomeConfiguration : IEntityTypeConfiguration<DecisionOutcome>
    {
        public void Configure(EntityTypeBuilder<DecisionOutcome> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "DecisionOutcomes",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.DecisionRecordId);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength);

            builder.Property(x => x.LessonsLearned)
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength);

            // Value Object Collection: OutcomeImpact (stored as JSON)
            builder.Property(x => x.Impacts)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<OutcomeImpact>>(v, (JsonSerializerOptions)null) ?? new List<OutcomeImpact>())
                .HasColumnName("Impacts");

            builder.HasIndex(x => x.ImplementationDate);
        }
    }
}
