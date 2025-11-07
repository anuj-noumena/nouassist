using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ReflectionAndSupport.ActivityTracking.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore.Configurations
{
    public class EntityHistoryConfiguration : IEntityTypeConfiguration<EntityHistory>
    {
        public void Configure(EntityTypeBuilder<EntityHistory> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ActivityDbProperties.DbTablePrefix + "EntityHistories", 
                           ActivityDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.EntityId).IsRequired();
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.ActorId).IsRequired();

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ActorName)
                .HasMaxLength(256);

            builder.Property(x => x.ChangeDescription)
                .HasMaxLength(2000);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(64);

            // JSON columns
            builder.Property(x => x.PreviousState)
                .HasColumnType("jsonb");

            builder.Property(x => x.CurrentState)
                .HasColumnType("jsonb");

            builder.Property(x => x.Changes)
                .HasColumnType("jsonb");

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
            builder.HasIndex(x => x.Version);
            builder.HasIndex(x => x.OccurredAt);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
