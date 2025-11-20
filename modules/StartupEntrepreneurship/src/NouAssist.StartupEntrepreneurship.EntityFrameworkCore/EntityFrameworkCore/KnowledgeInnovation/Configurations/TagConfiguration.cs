using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "Tags",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => new { x.Name, x.TenantId });

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Tag);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
