using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.Shared;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Budgets",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.PeriodStart).IsRequired();
            builder.Property(x => x.PeriodEnd).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // TotalBudget value object
            builder.OwnsOne(x => x.TotalBudget, tb =>
            {
                tb.Property(m => m.Amount)
                    .HasColumnName("TotalBudget")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                tb.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // ActualSpent value object
            builder.OwnsOne(x => x.ActualSpent, asp =>
            {
                asp.Property(m => m.Amount)
                    .HasColumnName("ActualSpent")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                asp.Property(m => m.Currency)
                    .HasColumnName("ActualSpentCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Navigation to allocations
            builder.HasMany(x => x.Allocations)
                .WithOne()
                .HasForeignKey("BudgetId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.PeriodStart, x.PeriodEnd });
            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_Budgets_User_Status");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
