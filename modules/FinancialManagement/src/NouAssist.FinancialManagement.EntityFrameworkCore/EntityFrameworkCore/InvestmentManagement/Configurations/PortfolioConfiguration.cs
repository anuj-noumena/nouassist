using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.InvestmentManagement.Configurations
{
    public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Portfolios",
                FinancialManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.RiskTolerance)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Financial metrics
            builder.Property(x => x.TotalValue).HasPrecision(18, 2);
            builder.Property(x => x.TotalCostBasis).HasPrecision(18, 2);
            builder.Property(x => x.TotalGainLoss).HasPrecision(18, 2);
            builder.Property(x => x.TotalGainLossPercentage).HasPrecision(5, 2);
            builder.Property(x => x.CashBalance).HasPrecision(18, 2);

            // Relationships - Holdings (owned collection)
            builder.OwnsMany(x => x.Holdings, holding =>
            {
                holding.ToTable(
                    InvestmentManagementDbProperties.DbTablePrefix + "Holdings",
                    InvestmentManagementDbProperties.DbSchema
                );

                holding.WithOwner().HasForeignKey("PortfolioId");
                holding.Property<Guid>("PortfolioId").IsRequired();

                holding.Property(h => h.Symbol)
                    .IsRequired()
                    .HasMaxLength(20);

                holding.Property(h => h.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                holding.Property(h => h.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                holding.Property(h => h.AssetClass)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                holding.Property(h => h.Quantity).HasPrecision(18, 8);
                holding.Property(h => h.AverageCostBasis).HasPrecision(18, 4);
                holding.Property(h => h.CurrentPrice).HasPrecision(18, 4);
                holding.Property(h => h.MarketValue).HasPrecision(18, 2);
                holding.Property(h => h.UtilizationPercentage).HasPrecision(5, 2);

                holding.HasKey("PortfolioId", nameof(Holding.Symbol));
                holding.HasIndex(h => h.Symbol);
            });

            // Relationships - Transactions (owned collection)
            builder.OwnsMany(x => x.Transactions, transaction =>
            {
                transaction.ToTable(
                    FinancialManagementDbProperties.DbTablePrefix + "InvestmentTransactions",
                    FinancialManagementDbProperties.DbSchema
                );

                transaction.WithOwner().HasForeignKey("PortfolioId");
                transaction.Property<Guid>("PortfolioId").IsRequired();

                transaction.HasKey(t => t.Id);

                transaction.Property(t => t.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                transaction.Property(t => t.Symbol)
                    .IsRequired()
                    .HasMaxLength(20);

                transaction.Property(t => t.Quantity).HasPrecision(18, 8);
                transaction.Property(t => t.Price).HasPrecision(18, 4);
                transaction.Property(t => t.TotalAmount).HasPrecision(18, 2);
                transaction.Property(t => t.Fees).HasPrecision(18, 2);
                transaction.Property(t => t.TransactionDate).IsRequired();
                transaction.Property(t => t.Notes).HasMaxLength(500);

                transaction.HasIndex(t => t.TransactionDate);
                transaction.HasIndex(t => new { t.Symbol, t.TransactionDate });
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.Status });
            builder.HasIndex(x => x.LastRebalanceDate);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
