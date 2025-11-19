using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Configurations
{
    public class ExpenseReportConfiguration : IEntityTypeConfiguration<ExpenseReport>
    {
        public void Configure(EntityTypeBuilder<ExpenseReport> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "ExpenseReports",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubmitterId).IsRequired();
            
            builder.Property(x => x.ReportName)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            builder.Property(x => x.PeriodStart).IsRequired();
            builder.Property(x => x.PeriodEnd).IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // TotalAmount value object
            builder.OwnsOne(x => x.TotalAmount, ta =>
            {
                ta.Property(m => m.Amount)
                    .HasColumnName("TotalAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ta.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.ApprovedBy);
            builder.Property(x => x.SubmittedDate);

            builder.HasIndex(x => x.SubmitterId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.SubmitterId, x.Status })
                .HasDatabaseName("IX_ExpenseReports_Submitter_Status");

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
