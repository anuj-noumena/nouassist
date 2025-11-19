using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.AccountsReceivable.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Customers",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            // TaxId value object
            builder.OwnsOne(x => x.TaxId, ti =>
            {
                ti.Property(t => t.Value)
                    .HasColumnName("TaxId")
                    .HasMaxLength(50);
                
                ti.Property(t => t.Country)
                    .HasColumnName("TaxIdCountry")
                    .HasMaxLength(2);
            });

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxEmailLength);

            builder.Property(x => x.Phone)
                .HasMaxLength(FinancialConsts.MaxPhoneLength);

            builder.Property(x => x.BillingAddress)
                .HasMaxLength(FinancialConsts.MaxAddressLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CreditTermsDays).IsRequired();

            // CreditLimit value object
            builder.OwnsOne(x => x.CreditLimit, cl =>
            {
                cl.Property(m => m.Amount)
                    .HasColumnName("CreditLimit")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                cl.Property(m => m.Currency)
                    .HasColumnName("CreditLimitCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // CurrentBalance value object
            builder.OwnsOne(x => x.CurrentBalance, cb =>
            {
                cb.Property(m => m.Amount)
                    .HasColumnName("CurrentBalance")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                cb.Property(m => m.Currency)
                    .HasColumnName("CurrentBalanceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreditOnHold).IsRequired();

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.TenantId, x.Name })
                .HasDatabaseName("IX_Customers_Tenant_Name");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
