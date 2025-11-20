using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(SharedKernelDbProperties.DbTablePrefix + "Areas",
                SharedKernelDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.Purpose)
                .HasMaxLength(2000);

            builder.Property(x => x.StandardsToMaintain)
                .HasColumnType("jsonb");

            builder.Property(x => x.KeyResultAreas)
                .HasColumnType("jsonb");

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            builder.Property(x => x.Icon)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.LastReviewedAt);

            builder.Property(x => x.ArchivedAt);

            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => x.ParentId);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
