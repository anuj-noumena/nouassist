using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.AccountsReceivable
{
    public class CollectionCaseConfiguration : IEntityTypeConfiguration<CollectionCase>
    {
        public void Configure(EntityTypeBuilder<CollectionCase> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "CollectionCases",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId).IsRequired();

            builder.OwnsOne(x => x.TotalOverdueAmount, total =>
            {
                total.Property(t => t.Amount).HasColumnName("TotalOverdueAmount").HasPrecision(18, 4).IsRequired();
                total.Property(t => t.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
                total.Ignore(t => t.DecimalPlaces);
            });

            builder.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.AssignedTo).IsRequired();
            builder.Property(x => x.LastContactDate);

            builder.HasMany<CollectionActivity>()
                .WithOne()
                .HasForeignKey(a => a.CaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_CollectionCases_CustomerId");
            builder.HasIndex(x => x.Status).HasDatabaseName("IX_CollectionCases_Status");
            builder.HasIndex(x => x.AssignedTo).HasDatabaseName("IX_CollectionCases_AssignedTo");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_CollectionCases_TenantId");
        }
    }
}
