using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore.Configurations
{
    public class ClientAggregateConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ContactsDbProperties.DbTablePrefix + "Clients", ContactsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            // Basic info
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.LegalName)
                .HasMaxLength(200);

            builder.Property(x => x.Industry)
                .HasMaxLength(100);

            builder.Property(x => x.Website)
                .HasMaxLength(500);

            // Contact info
            builder.Property(x => x.PrimaryContactId);
            builder.Property(x => x.Email).HasMaxLength(256);
            builder.Property(x => x.Phone).HasMaxLength(50);

            // Business details
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ClientSince);

            builder.Property(x => x.LifetimeValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CurrentBalance)
                .HasColumnType("decimal(18,2)");

            // Status
            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.InactiveSince);

            // Interaction tracking
            builder.Property(x => x.LastContactedAt);
            builder.Property(x => x.NextReviewDate);

            // Collection: Tags (stored as JSON array)
            builder.OwnsMany(x => x.Tags, tagBuilder =>
            {
                tagBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => new { x.OwnerId, x.Status });
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => x.NextReviewDate);
            builder.HasIndex(x => x.PrimaryContactId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
