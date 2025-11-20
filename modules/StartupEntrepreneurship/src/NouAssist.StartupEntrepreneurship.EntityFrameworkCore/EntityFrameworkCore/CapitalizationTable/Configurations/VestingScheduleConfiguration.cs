using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for VestingSchedule entity
    /// </summary>
    public class VestingScheduleConfiguration : IEntityTypeConfiguration<VestingSchedule>
    {
        public void Configure(EntityTypeBuilder<VestingSchedule> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "VestingSchedules", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.SecurityHolderId)
                .IsRequired();

            builder.Property(x => x.SecurityIssuanceId)
                .IsRequired();

            builder.Property(x => x.GrantDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LastVestingDate);
            builder.Property(x => x.ExercisedDate);

            // Value Object Mappings
            builder.OwnsOne(x => x.VestingTerms, vestingBuilder =>
            {
                vestingBuilder.Property(v => v.VestingPeriodMonths)
                    .HasColumnName("VestingPeriodMonths")
                    .IsRequired();
                vestingBuilder.Property(v => v.CliffMonths)
                    .HasColumnName("CliffMonths")
                    .IsRequired()
                    .HasDefaultValue(0);
                vestingBuilder.Property(v => v.VestingPercentage)
                    .HasColumnName("VestingPercentage")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired()
                    .HasDefaultValue(100);
            });

            builder.OwnsOne(x => x.TotalShares, totalBuilder =>
            {
                totalBuilder.Property(t => t.Value)
                    .HasColumnName("TotalShares")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.VestedShares, vestedBuilder =>
            {
                vestedBuilder.Property(v => v.Value)
                    .HasColumnName("VestedShares")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            // Indexing
            builder.HasIndex(x => x.SecurityHolderId);
            builder.HasIndex(x => x.SecurityIssuanceId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.GrantDate);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
