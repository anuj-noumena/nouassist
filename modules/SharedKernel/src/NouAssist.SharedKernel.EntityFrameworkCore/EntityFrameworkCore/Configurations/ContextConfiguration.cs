using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class ContextConfiguration : IEntityTypeConfiguration<Context>
    {
        public void Configure(EntityTypeBuilder<Context> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CoreDbProperties.DbTablePrefix + "Contexts",
                            CoreDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Notes)
                .HasMaxLength(10000);

            builder.Property(x => x.Icon)
                .HasMaxLength(50);

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            builder.Property(x => x.Location)
                .HasMaxLength(200);

            builder.Property(x => x.Tools)
                .HasColumnType("jsonb");

            builder.Property(x => x.EnergyLevel);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.UsageCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.LastUsedAt);

            builder.Property(x => x.ArchivedAt);

            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => x.Location);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
