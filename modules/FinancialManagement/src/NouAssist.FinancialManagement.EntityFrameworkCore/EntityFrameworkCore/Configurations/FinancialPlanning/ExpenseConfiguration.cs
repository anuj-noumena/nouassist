using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Expenses",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId).IsRequired();

            // MonetaryValue - Amount
            builder.OwnsOne(x => x.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                amount.Property(a => a.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                amount.Ignore(a => a.DecimalPlaces);
            });

            builder.Property(x => x.ExpenseDate).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.PayeeId).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ReceiptDocumentId);
            builder.Property(x => x.ProjectId);
            builder.Property(x => x.IsReimbursable).IsRequired();
            builder.Property(x => x.IsBillable).IsRequired();

            // MileageData - Value Object
            builder.OwnsOne(x => x.MileageInfo, mileage =>
            {
                mileage.Property(m => m.Miles)
                    .HasColumnName("Mileage_Miles")
                    .HasPrecision(18, 2);

                mileage.Property(m => m.RatePerMile)
                    .HasColumnName("Mileage_RatePerMile")
                    .HasPrecision(18, 4);

                mileage.Property(m => m.Purpose)
                    .HasColumnName("Mileage_Purpose")
                    .HasMaxLength(500);

                mileage.Property(m => m.StartLocation)
                    .HasColumnName("Mileage_StartLocation")
                    .HasMaxLength(200);

                mileage.Property(m => m.EndLocation)
                    .HasColumnName("Mileage_EndLocation")
                    .HasMaxLength(200);
            });

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Expenses_UserId");

            builder.HasIndex(x => x.ExpenseDate)
                .HasDatabaseName("IX_Expenses_ExpenseDate");

            builder.HasIndex(x => x.CategoryId)
                .HasDatabaseName("IX_Expenses_CategoryId");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Expenses_Status");

            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_Expenses_UserId_Status");

            builder.HasIndex(x => new { x.ExpenseDate, x.CategoryId })
                .HasDatabaseName("IX_Expenses_ExpenseDate_CategoryId");

            builder.HasIndex(x => x.ProjectId)
                .HasDatabaseName("IX_Expenses_ProjectId");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Expenses_TenantId");
        }
    }
}
