using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ShareClass entity
    /// </summary>
    public class ShareClassConfiguration : IEntityTypeConfiguration<ShareClass>
    {
        public void Configure(EntityTypeBuilder<ShareClass> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "ShareClasses", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxShareClassNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(CapTableConsts.MaxDescriptionLength);

            builder.Property(x => x.SecurityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ParValue)
                .HasColumnType("decimal(18,4)")
                .IsRequired()
                .HasDefaultValue(0.001m);

            builder.Property(x => x.VotingRights)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.HasDividendRights)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.HasLiquidationPreference)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.LiquidationMultiple)
                .HasColumnType("decimal(5,2)")
                .IsRequired()
                .HasDefaultValue(1.0m);

            builder.Property(x => x.HasAntiDilutionRights)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.AntiDilutionType)
                .HasMaxLength(50);

            // Indexing
            builder.HasIndex(x => x.SecurityType);
        }
    }
}
