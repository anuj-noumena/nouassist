using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Expenses",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            // Amount value object
            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                a.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.ExpenseDate).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.PayeeId).IsRequired();
            
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ReceiptDocumentId);
            builder.Property(x => x.ProjectId);
            builder.Property(x => x.IsReimbursable).IsRequired();
            builder.Property(x => x.IsBillable).IsRequired();

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ExpenseDate);
            builder.HasIndex(x => new { x.UserId, x.ExpenseDate })
                .HasDatabaseName("IX_Expenses_User_Date");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
