using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.TaxManagement.EntityFrameworkCore.Configurations
{
    public class TaxProfileConfiguration : IEntityTypeConfiguration<TaxProfile>
    {
        public void Configure(EntityTypeBuilder<TaxProfile> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TaxManagementDbProperties.DbTablePrefix + "TaxProfiles",
                TaxManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.DefaultFilingStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.NumberOfDependents)
                .IsRequired();

            builder.Property(x => x.IsItemizer)
                .IsRequired();

            builder.Property(x => x.IsSelfEmployed)
                .IsRequired();

            builder.Property(x => x.TaxPreparer)
                .HasMaxLength(200);

            // Store TaxSettings as JSON
            builder.Property(x => x.TaxSettings)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.UserId).IsUnique();

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
