using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.TaxManagement.EntityFrameworkCore.Configurations
{
    public class TaxReturnConfiguration : IEntityTypeConfiguration<TaxReturn>
    {
        public void Configure(EntityTypeBuilder<TaxReturn> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TaxManagementDbProperties.DbTablePrefix + "TaxReturns",
                TaxManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaxYear)
                .IsRequired();

            builder.Property(x => x.FilingStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Income properties
            builder.Property(x => x.W2Income).HasPrecision(18, 2);
            builder.Property(x => x.SelfEmploymentIncome).HasPrecision(18, 2);
            builder.Property(x => x.InvestmentIncome).HasPrecision(18, 2);
            builder.Property(x => x.OtherIncome).HasPrecision(18, 2);

            // Deduction properties
            builder.Property(x => x.StandardDeduction).HasPrecision(18, 2);
            builder.Property(x => x.TotalItemizedDeductions).HasPrecision(18, 2);
            builder.Property(x => x.AdjustedGrossIncome).HasPrecision(18, 2);
            builder.Property(x => x.TaxableIncome).HasPrecision(18, 2);

            // Tax liability properties
            builder.Property(x => x.TotalTaxLiability).HasPrecision(18, 2);
            builder.Property(x => x.WithholdingsPaid).HasPrecision(18, 2);
            builder.Property(x => x.EstimatedTaxPayments).HasPrecision(18, 2);
            builder.Property(x => x.RefundAmount).HasPrecision(18, 2);
            builder.Property(x => x.AmountOwed).HasPrecision(18, 2);

            builder.Property(x => x.ConfirmationNumber).HasMaxLength(100);

            // Relationships - Deductions (owned collection)
            builder.OwnsMany(x => x.Deductions, deduction =>
            {
                deduction.ToTable(
                    TaxManagementDbProperties.DbTablePrefix + "TaxDeductions",
                    TaxManagementDbProperties.DbSchema
                );

                deduction.WithOwner().HasForeignKey("TaxReturnId");
                deduction.Property<Guid>("TaxReturnId").IsRequired();

                deduction.Property(d => d.Category)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                deduction.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                deduction.Property(d => d.ClaimedAmount).HasPrecision(18, 2);
                deduction.Property(d => d.AllowedAmount).HasPrecision(18, 2);
                deduction.Property(d => d.Notes).HasMaxLength(1000);

                deduction.HasKey("TaxReturnId", nameof(TaxDeduction.Category), nameof(TaxDeduction.Description));
            });

            // Relationships - Documents (owned collection)
            builder.OwnsMany(x => x.Documents, document =>
            {
                document.ToTable(
                    TaxManagementDbProperties.DbTablePrefix + "TaxDocuments",
                    TaxManagementDbProperties.DbSchema
                );

                document.WithOwner().HasForeignKey("TaxReturnId");
                document.Property<Guid>("TaxReturnId").IsRequired();

                document.HasKey(d => d.Id);

                document.Property(d => d.FormType)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                document.Property(d => d.DocumentName)
                    .IsRequired()
                    .HasMaxLength(200);

                document.Property(d => d.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                document.Property(d => d.RejectionReason).HasMaxLength(500);
            });

            // Relationships - Estimated Payments (owned collection)
            builder.OwnsMany(x => x.EstimatedPayments, payment =>
            {
                payment.ToTable(
                    TaxManagementDbProperties.DbTablePrefix + "EstimatedTaxPayments",
                    TaxManagementDbProperties.DbSchema
                );

                payment.WithOwner().HasForeignKey("TaxReturnId");
                payment.Property<Guid>("TaxReturnId").IsRequired();

                payment.HasKey(p => p.Id);

                payment.Property(p => p.Quarter).IsRequired();
                payment.Property(p => p.DueDate).IsRequired();
                payment.Property(p => p.Amount).IsRequired().HasPrecision(18, 2);
                payment.Property(p => p.IsPaid).IsRequired();
                payment.Property(p => p.ConfirmationNumber).HasMaxLength(100);
            });

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.TaxYear }).IsUnique();
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.FiledDate);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
