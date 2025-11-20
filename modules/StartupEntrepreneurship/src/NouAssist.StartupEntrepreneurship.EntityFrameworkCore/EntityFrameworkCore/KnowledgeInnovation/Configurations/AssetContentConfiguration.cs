using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class AssetContentConfiguration : IEntityTypeConfiguration<AssetContent>
    {
        public void Configure(EntityTypeBuilder<AssetContent> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "AssetContents",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.KnowledgeAssetId, x.Version });

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ChangeLog)
                .HasMaxLength(1000);

            builder.HasIndex(x => new { x.KnowledgeAssetId, x.Version }).IsUnique();
        }
    }
}
