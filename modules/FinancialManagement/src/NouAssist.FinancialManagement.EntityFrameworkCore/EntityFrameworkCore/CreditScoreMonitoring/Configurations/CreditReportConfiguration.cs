using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.CreditScoreMonitoring.Configurations
{
    public class CreditReportConfiguration : IEntityTypeConfiguration<CreditReport>
    {
        public void Configure(EntityTypeBuilder<CreditReport> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CreditReports",
                FinancialManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Bureau)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ReportDate)
                .IsRequired();

            builder.Property(x => x.CreditScore)
                .IsRequired();

            builder.Property(x => x.ScoreRange)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Metrics
            builder.Property(x => x.TotalCreditLimit).HasPrecision(18, 2);
            builder.Property(x => x.TotalBalances).HasPrecision(18, 2);
            builder.Property(x => x.CreditUtilizationPercentage).HasPrecision(5, 2);

            // Relationships - Accounts (owned collection)
            builder.OwnsMany(x => x.Accounts, account =>
            {
                account.ToTable(
                    FinancialManagementDbProperties.DbTablePrefix + "CreditAccounts",
                    FinancialManagementDbProperties.DbSchema
                );

                account.WithOwner().HasForeignKey("CreditReportId");
                account.Property<Guid>("CreditReportId").IsRequired();

                account.Property(a => a.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                account.Property(a => a.CreditorName)
                    .IsRequired()
                    .HasMaxLength(200);

                account.Property(a => a.AccountType)
                    .IsRequired()
                    .HasMaxLength(100);

                account.Property(a => a.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                account.Property(a => a.CreditLimit).HasPrecision(18, 2);
                account.Property(a => a.CurrentBalance).HasPrecision(18, 2);
                account.Property(a => a.UtilizationPercentage).HasPrecision(5, 2);
                //account.Property(a => a.MarketValue).HasPrecision(18, 2);

                account.HasKey("CreditReportId", nameof(CreditAccount.AccountNumber));
            });

            // Relationships - Inquiries (owned collection)
            builder.OwnsMany(x => x.Inquiries, inquiry =>
            {
                inquiry.ToTable(
                    FinancialManagementDbProperties.DbTablePrefix + "CreditInquiries",
                    FinancialManagementDbProperties.DbSchema
                );

                inquiry.WithOwner().HasForeignKey("CreditReportId");
                inquiry.Property<Guid>("CreditReportId").IsRequired();

                inquiry.HasKey(i => i.Id);

                inquiry.Property(i => i.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                inquiry.Property(i => i.InquirerName)
                    .IsRequired()
                    .HasMaxLength(200);

                inquiry.Property(i => i.InquiryDate)
                    .IsRequired();

                inquiry.Property(i => i.Purpose).HasMaxLength(500);
            });

            // Relationships - Alerts (owned collection)
            builder.OwnsMany(x => x.Alerts, alert =>
            {
                alert.ToTable(
                    FinancialManagementDbProperties.DbTablePrefix + "CreditAlerts",
                    FinancialManagementDbProperties.DbSchema
                );

                alert.WithOwner().HasForeignKey("CreditReportId");
                alert.Property<Guid>("CreditReportId").IsRequired();

                alert.HasKey(a => a.Id);

                alert.Property(a => a.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                alert.Property(a => a.Message)
                    .IsRequired()
                    .HasMaxLength(1000);

                alert.Property(a => a.Severity)
                    .IsRequired()
                    .HasMaxLength(50);

                alert.Property(a => a.IsRead).IsRequired();
                alert.Property(a => a.IsResolved).IsRequired();
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.Bureau, x.ReportDate });
            builder.HasIndex(x => x.ReportDate);
            builder.HasIndex(x => x.CreditScore);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
