using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Activity;
using NouAssist.Infrastructure.Activity.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Infrastructure.EntityFrameworkCore.Activity.Configurations
{
    public class ActivityFeedConfiguration : IEntityTypeConfiguration<ActivityFeed>
    {
        public void Configure(EntityTypeBuilder<ActivityFeed> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(InfrastructureDbProperties.DbTablePrefix + "ActivityFeeds", 
                InfrastructureDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ActorId).IsRequired();
            builder.Property(x => x.EntityId).IsRequired();
            
            builder.Property(x => x.Action)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EntityTitle)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Visibility)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // JSON columns for dictionaries
            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb");

            builder.Property(x => x.Changes)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.ActorId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
            builder.HasIndex(x => x.OccurredAt);
            builder.HasIndex(x => new { x.ActorId, x.IsRead });

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);
        }
    }
}
