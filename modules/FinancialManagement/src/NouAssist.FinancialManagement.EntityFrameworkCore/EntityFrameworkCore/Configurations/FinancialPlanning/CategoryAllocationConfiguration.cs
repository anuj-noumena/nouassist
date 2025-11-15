using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.Shared;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class CategoryAllocationConfiguration : IEntityTypeConfiguration<CategoryAllocation>
    {
        public void Configure(EntityTypeBuilder<CategoryAllocation> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CategoryAllocations",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.BudgetId, x.CategoryId });

            builder.Property(x => x.BudgetId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();

            // BudgetedAmount value object
            builder.OwnsOne(x => x.BudgetedAmount, ba =>
            {
                ba.Property(m => m.Amount)
                    .HasColumnName("BudgetedAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ba.Property(m => m.Currency)
                    .HasColumnName("BudgetedAmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // ActualAmount value object
            builder.OwnsOne(x => x.ActualAmount, aa =>
            {
                aa.Property(m => m.Amount)
                    .HasColumnName("ActualAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                aa.Property(m => m.Currency)
                    .HasColumnName("ActualAmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Indexes
            builder.HasIndex(x => x.BudgetId);
            builder.HasIndex(x => x.CategoryId);
        }
    }
}
