using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SharedKernel.Aggregates;

namespace NouAssist.SharedKernel.EntityFrameworkCore.Configurations
{
    public class LogActivityConfiguration : IEntityTypeConfiguration<LogActivity>
    {
        public void Configure(EntityTypeBuilder<LogActivity> builder)
        {
            builder.ToTable(SharedKernelDbProperties.DbTablePrefix + "LogActivities",
                SharedKernelDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityType)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.MetadataJson)
                .HasColumnType("jsonb");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(50);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.Property(x => x.OccurredAt)
                .IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OccurredAt);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
        }
    }
}
