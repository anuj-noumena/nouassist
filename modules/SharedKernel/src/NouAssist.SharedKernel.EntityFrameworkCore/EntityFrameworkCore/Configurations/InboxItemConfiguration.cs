using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class InboxItemConfiguration : IEntityTypeConfiguration<InboxItem>
    {
        public void Configure(EntityTypeBuilder<InboxItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(SharedKernelDbProperties.DbTablePrefix + "InboxItems",
                SharedKernelDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.InboxContent)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(x => x.ItemType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Source)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Processed)
                .IsRequired();

            builder.Property(x => x.ProcessedAt);

            builder.Property(x => x.ProcessedToEntityType)
                .HasMaxLength(100);

            builder.Property(x => x.ProcessedBy);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.HasIndex(x => new { x.OwnerId, x.Processed });
            builder.HasIndex(x => x.ProcessedAt);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
