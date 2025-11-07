using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Contacts.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Contacts.EntityFrameworkCore.Configurations
{
    public class ContactAggregateConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ContactsDbProperties.DbTablePrefix + "Contacts", ContactsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            // Name fields
            builder.Property(x => x.FirstName)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);

            builder.Property(x => x.OrganizationName)
                .HasMaxLength(200);

            builder.Property(x => x.DisplayName)
                .HasMaxLength(200);

            // Professional
            builder.Property(x => x.JobTitle)
                .HasMaxLength(200);

            builder.Property(x => x.Department)
                .HasMaxLength(200);

            builder.Property(x => x.ClientId);

            // Contact info
            builder.Property(x => x.PrimaryEmail)
                .HasMaxLength(256);

            builder.Property(x => x.PrimaryPhone)
                .HasMaxLength(50);

            builder.Property(x => x.Website)
                .HasMaxLength(500);

            // Categorization
            builder.Property(x => x.ContactType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Relationship)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Importance)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Status
            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.ArchivedAt);

            // Interaction tracking
            builder.Property(x => x.LastContactedAt);
            builder.Property(x => x.LastMetAt);
            builder.Property(x => x.ContactFrequencyDays).IsRequired();
            builder.Property(x => x.NextFollowupDate);

            // Value Objects: ContactMethods (stored as JSON)
            builder.OwnsMany(x => x.ContactMethods, cmBuilder =>
            {
                cmBuilder.ToJson();
                cmBuilder.Property(cm => cm.Type).IsRequired();
                cmBuilder.Property(cm => cm.Value).IsRequired();
            });

            // Value Objects: SocialProfiles (stored as JSON)
            builder.OwnsMany(x => x.SocialProfiles, spBuilder =>
            {
                spBuilder.ToJson();
                spBuilder.Property(sp => sp.Platform).IsRequired().HasConversion<string>();
                spBuilder.Property(sp => sp.Username).IsRequired();
            });

            // Collection: Tags (stored as JSON array)
            builder.OwnsMany(x => x.Tags, tagBuilder =>
            {
                tagBuilder.ToJson();
            });

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.PrimaryEmail);
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => new { x.OwnerId, x.LastContactedAt });
            builder.HasIndex(x => x.NextFollowupDate);
            builder.HasIndex(x => x.ClientId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
