using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class CategoryAllocationConfiguration : IEntityTypeConfiguration<CategoryAllocation>
    {
        public void Configure(EntityTypeBuilder<CategoryAllocation> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CategoryAllocations",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.BudgetId, x.CategoryId });

            // Properties
            builder.Property(x => x.BudgetId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();

            // MonetaryValue - BudgetedAmount
            builder.OwnsOne(x => x.BudgetedAmount, budgeted =>
            {
                budgeted.Property(b => b.Amount)
                    .HasColumnName("BudgetedAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                budgeted.Property(b => b.Currency)
                    .HasColumnName("BudgetedCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                budgeted.Ignore(b => b.DecimalPlaces);
            });

            // MonetaryValue - ActualAmount
            builder.OwnsOne(x => x.ActualAmount, actual =>
            {
                actual.Property(a => a.Amount)
                    .HasColumnName("ActualAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                actual.Property(a => a.Currency)
                    .HasColumnName("ActualCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                actual.Ignore(a => a.DecimalPlaces);
            });

            builder.Property(x => x.PercentageOfTotal)
                .HasPrecision(5, 2);

            // Indexes
            builder.HasIndex(x => x.BudgetId)
                .HasDatabaseName("IX_CategoryAllocations_BudgetId");

            builder.HasIndex(x => x.CategoryId)
                .HasDatabaseName("IX_CategoryAllocations_CategoryId");
        }
    }
}
