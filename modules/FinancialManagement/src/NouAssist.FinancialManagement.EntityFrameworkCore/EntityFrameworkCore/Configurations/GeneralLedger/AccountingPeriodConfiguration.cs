using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriod>
    {
        public void Configure(EntityTypeBuilder<AccountingPeriod> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "AccountingPeriods",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FiscalYear).IsRequired();
            builder.Property(x => x.PeriodNumber).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ClosedDate);
            builder.Property(x => x.ClosedBy);

            // Indexes
            builder.HasIndex(x => new { x.FiscalYear, x.PeriodNumber })
                .IsUnique()
                .HasDatabaseName("IX_AccountingPeriods_FiscalYear_PeriodNumber");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_AccountingPeriods_Status");

            builder.HasIndex(x => new { x.StartDate, x.EndDate })
                .HasDatabaseName("IX_AccountingPeriods_StartDate_EndDate");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_AccountingPeriods_TenantId");
        }
    }
}
