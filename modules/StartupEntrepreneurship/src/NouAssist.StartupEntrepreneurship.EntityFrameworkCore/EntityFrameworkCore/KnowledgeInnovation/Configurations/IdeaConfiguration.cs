using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
    {
        public void Configure(EntityTypeBuilder<Idea> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "Ideas",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.Idea.TitleMaxLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.Idea.DescriptionMaxLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SubmittedDate)
                .IsRequired();

            builder.Property(x => x.ImpactStatement)
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.ShortDescription);

            builder.Property(x => x.Priority)
                .IsRequired();

            // Value Object Mapping: IdeaSource
            builder.OwnsOne(x => x.Source, sourceBuilder =>
            {
                sourceBuilder.Property(s => s.SourceName).HasColumnName("SourceName")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Name)
                    .IsRequired();
                
                sourceBuilder.Property(s => s.SourceDescription).HasColumnName("SourceDescription")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.ShortDescription);
                
                sourceBuilder.Property(s => s.SourcedDate).HasColumnName("SourcedDate")
                    .IsRequired();
            });

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            // Relationship Mapping
            builder.HasMany(x => x.Evaluations)
                .WithOne()
                .HasForeignKey(v => v.IdeaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
