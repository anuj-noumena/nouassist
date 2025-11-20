using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for SecurityHolder aggregate
    /// </summary>
    public class SecurityHolderAggregateConfiguration : IEntityTypeConfiguration<SecurityHolder>
    {
        public void Configure(EntityTypeBuilder<SecurityHolder> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "SecurityHolders", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxHolderNameLength);

            builder.Property(x => x.Email)
                .HasMaxLength(256);

            builder.Property(x => x.HolderType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TaxId)
                .HasMaxLength(50);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.DateJoined);

            builder.Property(x => x.ExternalReference)
                .HasMaxLength(100);

            // Relationships - One-to-Many for vesting schedules
            builder.HasMany(x => x.VestingSchedules)
                   .WithOne()
                   .HasForeignKey(v => v.SecurityHolderId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.HolderType);
            builder.HasIndex(x => x.IsActive);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
