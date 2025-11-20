using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Entities;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class EntityTagConfiguration : IEntityTypeConfiguration<EntityTag>
    {
        public void Configure(EntityTypeBuilder<EntityTag> builder)
        {
            builder.ToTable(SharedKernelDbProperties.DbTablePrefix + "EntityTags",
                SharedKernelDbProperties.DbSchema);

            builder.HasKey(x => new { x.EntityType, x.EntityId, x.TagId });

            builder.Property(x => x.EntityType)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.TagId)
                .IsRequired();

            builder.Property(x => x.TaggedAt)
                .IsRequired();

            builder.HasIndex(x => x.TagId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
        }
    }
}
