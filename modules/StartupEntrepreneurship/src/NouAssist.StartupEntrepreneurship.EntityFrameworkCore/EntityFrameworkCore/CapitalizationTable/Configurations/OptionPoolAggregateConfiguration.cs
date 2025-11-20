using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for OptionPool aggregate
    /// </summary>
    public class OptionPoolAggregateConfiguration : IEntityTypeConfiguration<OptionPool>
    {
        public void Configure(EntityTypeBuilder<OptionPool> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CapTableDbProperties.DbTablePrefix + "OptionPools", CapTableDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.CapTableId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CapTableConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(CapTableConsts.MaxDescriptionLength);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            // Value Object Mappings
            builder.OwnsOne(x => x.TotalReserved, reservedBuilder =>
            {
                reservedBuilder.Property(r => r.Value)
                    .HasColumnName("TotalReserved")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.TotalGranted, grantedBuilder =>
            {
                grantedBuilder.Property(g => g.Value)
                    .HasColumnName("TotalGranted")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.TotalExercised, exercisedBuilder =>
            {
                exercisedBuilder.Property(e => e.Value)
                    .HasColumnName("TotalExercised")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.TotalForfeited, forfeitedBuilder =>
            {
                forfeitedBuilder.Property(f => f.Value)
                    .HasColumnName("TotalForfeited")
                    .HasColumnType("decimal(18,4)")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            builder.OwnsOne(x => x.PoolPercentage, percentageBuilder =>
            {
                percentageBuilder.Property(p => p.Value)
                    .HasColumnName("PoolPercentage")
                    .HasColumnType("decimal(5,4)")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.DefaultVestingTerms, vestingBuilder =>
            {
                vestingBuilder.Property(v => v.VestingPeriodMonths)
                    .HasColumnName("DefaultVestingPeriodMonths")
                    .IsRequired();
                vestingBuilder.Property(v => v.CliffMonths)
                    .HasColumnName("DefaultCliffMonths")
                    .IsRequired()
                    .HasDefaultValue(0);
                vestingBuilder.Property(v => v.VestingPercentage)
                    .HasColumnName("DefaultVestingPercentage")
                    .HasColumnType("decimal(5,2)")
                    .IsRequired()
                    .HasDefaultValue(100);
            });

            // Foreign Key to CapTable
            builder.HasOne<Aggregates.CapTable>()
                   .WithMany()
                   .HasForeignKey(x => x.CapTableId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.CapTableId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.CreatedDate);

            // Multi-tenancy support
            builder.HasIndex(x => x.TenantId);
        }
    }
}
