using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.Shared;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.GeneralLedger
{
    public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriod>
    {
        public void Configure(EntityTypeBuilder<AccountingPeriod> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "AccountingPeriods",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

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
            builder.HasIndex(x => new { x.TenantId, x.FiscalYear, x.PeriodNumber })
                .IsUnique()
                .HasDatabaseName("IX_AccountingPeriods_Tenant_Fiscal_Period");

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.StartDate, x.EndDate });

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
