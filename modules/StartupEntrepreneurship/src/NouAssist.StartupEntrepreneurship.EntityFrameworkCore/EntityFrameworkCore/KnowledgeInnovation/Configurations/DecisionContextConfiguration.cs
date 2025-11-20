using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class DecisionContextConfiguration : IEntityTypeConfiguration<DecisionContext>
    {
        public void Configure(EntityTypeBuilder<DecisionContext> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "DecisionContexts",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.DecisionRecordId);

            builder.Property(x => x.Background)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);

            builder.Property(x => x.ProblemStatement)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);

            builder.Property(x => x.ContextCapturedAt)
                .IsRequired();

            // Lists of strings stored as JSON
            builder.Property(x => x.Stakeholders)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Stakeholders");

            builder.Property(x => x.Constraints)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Constraints");
        }
    }
}
