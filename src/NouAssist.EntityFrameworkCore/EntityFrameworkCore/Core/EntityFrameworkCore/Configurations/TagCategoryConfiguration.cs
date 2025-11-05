using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Core.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Core.EntityFrameworkCore.Configurations
{
    public class TagCategoryConfiguration : IEntityTypeConfiguration<TagCategory>
    {
        public void Configure(EntityTypeBuilder<TagCategory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CoreDbProperties.DbTablePrefix + "TagCategories",
                            CoreDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            builder.HasIndex(x => new { x.OwnerId, x.Name });

            builder.ApplyObjectExtensionMappings();
        }
    }
}
