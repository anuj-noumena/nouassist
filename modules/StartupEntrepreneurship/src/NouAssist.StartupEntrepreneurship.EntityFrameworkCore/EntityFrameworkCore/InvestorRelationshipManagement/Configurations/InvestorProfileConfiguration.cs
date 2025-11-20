using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InvestorProfile aggregate
    /// </summary>
    public class InvestorProfileConfiguration : IEntityTypeConfiguration<InvestorProfile>
    {
        public void Configure(EntityTypeBuilder<InvestorProfile> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(IRMDbProperties.DbTablePrefix + "InvestorProfiles", IRMDbProperties.DbSchema);

            // Primary Key (handled by AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(IRMConsts.InvestorProfile.MaxNameLength);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(IRMConsts.InvestorProfile.MaxNameLength);

            builder.Property(x => x.CompanyName)
                .HasMaxLength(IRMConsts.InvestorProfile.MaxCompanyNameLength);

            builder.Property(x => x.Title)
                .HasMaxLength(200);

            builder.Property(x => x.Notes)
                .HasMaxLength(IRMConsts.InvestorProfile.MaxNotesLength);

            // Enum Mappings
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mappings - ContactDetails
            builder.OwnsOne(x => x.ContactDetails, contactBuilder =>
            {
                contactBuilder.Property(c => c.Email)
                    .IsRequired()
                    .HasColumnName("ContactEmail")
                    .HasMaxLength(IRMConsts.InvestorProfile.MaxEmailLength);

                contactBuilder.Property(c => c.Phone)
                    .HasColumnName("ContactPhone")
                    .HasMaxLength(IRMConsts.InvestorProfile.MaxPhoneLength);

                contactBuilder.Property(c => c.LinkedIn)
                    .HasColumnName("ContactLinkedIn")
                    .HasMaxLength(IRMConsts.InvestorProfile.MaxLinkedInLength);

                contactBuilder.Property(c => c.Website)
                    .HasColumnName("ContactWebsite")
                    .HasMaxLength(IRMConsts.InvestorProfile.MaxWebsiteLength);

                contactBuilder.Property(c => c.Address)
                    .HasColumnName("ContactAddress")
                    .HasMaxLength(500);
            });

            // Value Object Mappings - CommunicationPreferences
            builder.OwnsOne(x => x.CommunicationPreferences, prefBuilder =>
            {
                prefBuilder.Property(p => p.PreferredChannel)
                    .IsRequired()
                    .HasColumnName("PreferredCommunicationChannel")
                    .HasConversion<string>()
                    .HasMaxLength(50);

                prefBuilder.Property(p => p.UpdateFrequency)
                    .IsRequired()
                    .HasColumnName("UpdateFrequency")
                    .HasConversion<string>()
                    .HasMaxLength(50);

                prefBuilder.Property(p => p.AllowMarketing)
                    .HasColumnName("AllowMarketing")
                    .HasDefaultValue(true);

                prefBuilder.Property(p => p.Tags)
                    .HasColumnName("CommunicationTags")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .HasMaxLength(1000);
            });

            // Value Object Mappings - RelationshipScore
            builder.OwnsOne(x => x.RelationshipScore, scoreBuilder =>
            {
                scoreBuilder.Property(s => s.Score)
                    .IsRequired()
                    .HasColumnName("RelationshipScore");

                scoreBuilder.Property(s => s.LastCalculated)
                    .IsRequired()
                    .HasColumnName("RelationshipScoreCalculatedAt");

                scoreBuilder.Property(s => s.Notes)
                    .HasColumnName("RelationshipScoreNotes")
                    .HasMaxLength(500);
            });

            // One-to-Many Relationships within aggregate boundary
            builder.HasMany<InvestorInteraction>()
                .WithOne()
                .HasForeignKey("InvestorId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<InvestmentRecord>()
                .WithOne()
                .HasForeignKey("InvestorId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex("ContactDetails_Email")
                .HasDatabaseName("IX_InvestorProfiles_Email")
                .IsUnique();

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_InvestorProfiles_Status");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_InvestorProfiles_Type");

            builder.HasIndex(x => x.LastInteractionDate)
                .HasDatabaseName("IX_InvestorProfiles_LastInteraction");

            // Extra Properties (handled by ABP conventions)
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(InvestorProfile.ExtraProperties));

            // Concurrency Stamp (handled by ConfigureByConvention)
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
