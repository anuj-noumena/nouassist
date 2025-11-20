using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class StakeholderProfileConfiguration : IEntityTypeConfiguration<StakeholderProfile>
    {
        public void Configure(EntityTypeBuilder<StakeholderProfile> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "StakeholderProfiles", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Basic Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ElicitationConsts.MaxStakeholderNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ElicitationConsts.MaxStakeholderDescriptionLength);

            builder.Property(x => x.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Organization)
                .HasMaxLength(ElicitationConsts.MaxOrganizationLength);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // ContactInfo Value Object
            builder.OwnsOne(x => x.ContactInfo, contactBuilder =>
            {
                contactBuilder.Property(c => c.Email)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasMaxLength(ElicitationConsts.MaxEmailLength);

                contactBuilder.Property(c => c.Phone)
                    .HasColumnName("Phone")
                    .HasMaxLength(ElicitationConsts.MaxPhoneLength);

                contactBuilder.Property(c => c.Address)
                    .HasColumnName("Address")
                    .HasMaxLength(ElicitationConsts.MaxAddressLength);
            });

            // InfluenceData Value Object
            builder.OwnsOne(x => x.InfluenceData, influenceBuilder =>
            {
                influenceBuilder.Property(i => i.InfluenceLevel)
                    .IsRequired()
                    .HasColumnName("InfluenceLevel");

                influenceBuilder.Property(i => i.InterestLevel)
                    .IsRequired()
                    .HasColumnName("InterestLevel");

                influenceBuilder.Property(i => i.Notes)
                    .HasColumnName("InfluenceNotes")
                    .HasMaxLength(500);
            });

            // Indexes
            builder.HasIndex(x => x.ContactInfo.Email)
                .IsUnique()
                .HasDatabaseName("IX_StakeholderProfiles_Email");

            builder.HasIndex(x => x.Role)
                .HasDatabaseName("IX_StakeholderProfiles_Role");

            builder.HasIndex(x => x.Organization)
                .HasDatabaseName("IX_StakeholderProfiles_Organization");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_StakeholderProfiles_IsActive");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(StakeholderProfile.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
