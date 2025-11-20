using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class TagCategoryConfiguration : IEntityTypeConfiguration<TagCategory>
    {
        public void Configure(EntityTypeBuilder<TagCategory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(SharedKernelDbProperties.DbTablePrefix + "TagCategories",
                SharedKernelDbProperties.DbSchema);

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
