using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CoreDbProperties.DbTablePrefix + "Tags",
                            CoreDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            builder.Property(x => x.UsageCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(x => new { x.OwnerId, x.Title });
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.UsageCount);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
