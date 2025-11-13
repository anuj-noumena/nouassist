using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Customers",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TaxId)
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .HasMaxLength(50);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // CreditTerms - Value Object
            builder.OwnsOne(x => x.CreditTerms, terms =>
            {
                terms.Property(t => t.DueDays)
                    .HasColumnName("CreditTerms_DueDays")
                    .IsRequired();

                terms.Property(t => t.CreditLimit)
                    .HasColumnName("CreditTerms_CreditLimit")
                    .HasPrecision(18, 4)
                    .IsRequired();

                terms.Property(t => t.Currency)
                    .HasColumnName("CreditTerms_Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // MonetaryValue - CurrentBalance
            builder.OwnsOne(x => x.CurrentBalance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("CurrentBalance_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                balance.Property(b => b.Currency)
                    .HasColumnName("CurrentBalance_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                balance.Ignore(b => b.DecimalPlaces);
            });

            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsCreditSuspended).IsRequired();

            // Indexes
            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_Customers_Name");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_Customers_Email");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_Customers_Type");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_Customers_IsActive");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Customers_TenantId");
        }
    }
}
