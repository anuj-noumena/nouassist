using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class DecisionRecordConfiguration : IEntityTypeConfiguration<DecisionRecord>
    {
        public void Configure(EntityTypeBuilder<DecisionRecord> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "DecisionRecords",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.TitleMaxLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.DecisionDate)
                .IsRequired();

            // Value Object Mapping: DecisionRationale
            builder.OwnsOne(x => x.Rationale, rationaleBuilder =>
            {
                rationaleBuilder.Property(r => r.Reasoning).HasColumnName("Reasoning")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DecisionRecord.RationaleMaxLength);
                
                rationaleBuilder.Property(r => r.KeyFactors).HasColumnName("KeyFactors")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>());
                
                rationaleBuilder.Property(r => r.RisksIdentified).HasColumnName("RisksIdentified")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>());
                
                rationaleBuilder.Property(r => r.AssumptionsMade).HasColumnName("AssumptionsMade")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>());
            });

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            // Relationship Mappings
            builder.HasOne(x => x.Context)
                .WithOne()
                .HasForeignKey<DecisionContext>(c => c.DecisionRecordId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Alternatives)
                .WithOne()
                .HasForeignKey(a => a.DecisionRecordId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Outcome)
                .WithOne()
                .HasForeignKey<DecisionOutcome>(o => o.DecisionRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DecisionDate);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
