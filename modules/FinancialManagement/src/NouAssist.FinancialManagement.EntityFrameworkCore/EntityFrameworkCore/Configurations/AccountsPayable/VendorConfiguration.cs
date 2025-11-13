using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsPayable
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "Vendors",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TaxId)
                .HasMaxLength(50);

            // ContactInfo - Value Object
            builder.OwnsOne(x => x.ContactInfo, contact =>
            {
                contact.Property(c => c.Email)
                    .HasColumnName("ContactEmail")
                    .HasMaxLength(200)
                    .IsRequired();

                contact.Property(c => c.Phone)
                    .HasColumnName("ContactPhone")
                    .HasMaxLength(50);

                contact.Property(c => c.Address)
                    .HasColumnName("ContactAddress")
                    .HasMaxLength(500);

                contact.Property(c => c.City)
                    .HasColumnName("ContactCity")
                    .HasMaxLength(100);

                contact.Property(c => c.State)
                    .HasColumnName("ContactState")
                    .HasMaxLength(50);

                contact.Property(c => c.ZipCode)
                    .HasColumnName("ContactZipCode")
                    .HasMaxLength(20);

                contact.Property(c => c.Country)
                    .HasColumnName("ContactCountry")
                    .HasMaxLength(100);
            });

            // PaymentTerms - Value Object
            builder.OwnsOne(x => x.DefaultTerms, terms =>
            {
                terms.Property(t => t.NetDays)
                    .HasColumnName("PaymentTerms_NetDays")
                    .IsRequired();

                terms.Property(t => t.DiscountDays)
                    .HasColumnName("PaymentTerms_DiscountDays");

                terms.Property(t => t.DiscountPercentage)
                    .HasColumnName("PaymentTerms_DiscountPercentage")
                    .HasPrecision(5, 2);
            });

            builder.Property(x => x.PreferredMethod)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive).IsRequired();

            // MonetaryValue - CreditLimit
            builder.OwnsOne(x => x.CreditLimit, credit =>
            {
                credit.Property(c => c.Amount)
                    .HasColumnName("CreditLimit_Amount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                credit.Property(c => c.Currency)
                    .HasColumnName("CreditLimit_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                credit.Ignore(c => c.DecimalPlaces);
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

            // Indexes
            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_Vendors_Name");

            builder.HasIndex(x => x.TaxId)
                .HasDatabaseName("IX_Vendors_TaxId");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_Vendors_IsActive");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_Vendors_TenantId");
        }
    }
}
