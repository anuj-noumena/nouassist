using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class LearningResourceConfiguration : IEntityTypeConfiguration<LearningResource>
    {
        public void Configure(EntityTypeBuilder<LearningResource> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "LearningResources",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.LearningResource.TitleMaxLength);

            builder.Property(x => x.ResourceType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ResourceUrl)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Url);

            builder.Property(x => x.IsPublished)
                .IsRequired();

            builder.Property(x => x.IsFeatured)
                .IsRequired();

            // Value Object Mapping: LearningModuleDescription
            builder.OwnsOne(x => x.Description, descBuilder =>
            {
                descBuilder.Property(d => d.Overview).HasColumnName("Overview")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength)
                    .IsRequired();
                
                descBuilder.Property(d => d.LearningObjectives).HasColumnName("LearningObjectives")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength)
                    .IsRequired();
                
                descBuilder.Property(d => d.PrerequisiteKnowledge).HasColumnName("PrerequisiteKnowledge")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.LearningResource.DescriptionMaxLength);
                
                descBuilder.Property(d => d.EstimatedDurationMinutes).HasColumnName("EstimatedDurationMinutes")
                    .IsRequired();
            });

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            // Relationship Mapping
            builder.HasMany(x => x.UserProgressRecords)
                .WithOne()
                .HasForeignKey(v => v.LearningResourceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.ResourceType);
            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => x.IsFeatured);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
