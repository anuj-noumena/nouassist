using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.DebtManagement.Configurations
{
    public class DebtPayoffPlanConfiguration : IEntityTypeConfiguration<DebtPayoffPlan>
    {
        public void Configure(EntityTypeBuilder<DebtPayoffPlan> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "DebtPayoffPlans",
                FinancialManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PlanName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Strategy)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.MonthlyPaymentAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Relationships - PayoffPlanDebts (owned collection)
            builder.OwnsMany(x => x.Debts, debt =>
            {
                debt.ToTable(
                    FinancialManagementDbProperties.DbTablePrefix + "PayoffPlanDebts",
                    FinancialManagementDbProperties.DbSchema
                );

                debt.WithOwner().HasForeignKey(nameof(DebtPayoffPlan) + "Id");

                debt.Property<Guid>(nameof(DebtPayoffPlan) + "Id").IsRequired();
                
                debt.Property(d => d.DebtAccountId).IsRequired();
                debt.Property(d => d.PayoffOrder).IsRequired();
                debt.Property(d => d.AllocatedMonthlyPayment).IsRequired().HasPrecision(18, 2);
                debt.Property(d => d.IsIncluded).IsRequired();

                debt.HasKey(nameof(DebtPayoffPlan) + "Id", nameof(PayoffPlanDebt.DebtAccountId));

                debt.HasIndex(d => d.PayoffOrder);
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.IsActive });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
