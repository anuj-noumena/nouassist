using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Budgets",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

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

            // MonetaryValue - TotalBudget
            builder.OwnsOne(x => x.TotalBudget, total =>
            {
                total.Property(t => t.Amount)
                    .HasColumnName("TotalBudget_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                total.Property(t => t.Currency)
                    .HasColumnName("TotalBudget_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                total.Ignore(t => t.DecimalPlaces);
            });

            // MonetaryValue - ActualSpent
            builder.OwnsOne(x => x.ActualSpent, actual =>
            {
                actual.Property(a => a.Amount)
                    .HasColumnName("ActualSpent_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                actual.Property(a => a.Currency)
                    .HasColumnName("ActualSpent_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                actual.Ignore(a => a.DecimalPlaces);
            });

            // Relationships
            builder.HasMany<CategoryAllocation>()
                .WithOne()
                .HasForeignKey(a => a.BudgetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Budgets_UserId");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_Budgets_Type");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Budgets_Status");

            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_Budgets_UserId_Status");

            builder.HasIndex(x => new { x.PeriodStart, x.PeriodEnd })
                .HasDatabaseName("IX_Budgets_Period");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Budgets_TenantId");
        }
    }
}
