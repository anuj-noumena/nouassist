using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class GoalContributionConfiguration : IEntityTypeConfiguration<GoalContribution>
    {
        public void Configure(EntityTypeBuilder<GoalContribution> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "GoalContributions",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => new { x.GoalId, x.ContributionDate });

            // Properties
            builder.Property(x => x.GoalId).IsRequired();

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

            builder.Property(x => x.ContributionDate).IsRequired();
            builder.Property(x => x.Notes).HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.GoalId)
                .HasDatabaseName("IX_GoalContributions_GoalId");

            builder.HasIndex(x => x.ContributionDate)
                .HasDatabaseName("IX_GoalContributions_ContributionDate");
        }
    }
}
