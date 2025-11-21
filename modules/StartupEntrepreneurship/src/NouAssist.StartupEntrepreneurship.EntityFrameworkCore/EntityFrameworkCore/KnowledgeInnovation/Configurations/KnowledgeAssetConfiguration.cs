using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class KnowledgeAssetConfiguration : IEntityTypeConfiguration<KnowledgeAsset>
    {
        public void Configure(EntityTypeBuilder<KnowledgeAsset> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "KnowledgeAssets",
                            KnowledgeInnovationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.KnowledgeAsset.TitleMaxLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.KnowledgeAsset.DescriptionMaxLength);

            // Enum Mapping
            builder.Property(x => x.AssetType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping: AuthorInfo
            builder.OwnsOne(x => x.Author, authorBuilder =>
            {
                authorBuilder.Property(a => a.UserId).HasColumnName("AuthorUserId").IsRequired();
                authorBuilder.Property(a => a.Name).HasColumnName("AuthorName")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Name)
                    .IsRequired();
                authorBuilder.Property(a => a.Title).HasColumnName("AuthorTitle")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Title);
                authorBuilder.Property(a => a.AvatarUrl).HasColumnName("AuthorAvatarUrl")
                    .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Url);
            });

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            // Relationship Mapping (One-to-Many)
            builder.HasMany(x => x.ContentVersions)
                .WithOne()
                .HasForeignKey(v => v.KnowledgeAssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Index definitions
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.AssetType);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
