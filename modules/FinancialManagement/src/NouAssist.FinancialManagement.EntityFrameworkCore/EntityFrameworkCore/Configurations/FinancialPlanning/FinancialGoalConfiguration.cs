using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Configurations.FinancialPlanning
{
    public class FinancialGoalConfiguration : IEntityTypeConfiguration<FinancialGoal>
    {
        public void Configure(EntityTypeBuilder<FinancialGoal> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "FinancialGoals",
                FinancialManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // MonetaryValue - TargetAmount
            builder.OwnsOne(x => x.TargetAmount, target =>
            {
                target.Property(t => t.Amount)
                    .HasColumnName("TargetAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                target.Property(t => t.Currency)
                    .HasColumnName("TargetCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                target.Ignore(t => t.DecimalPlaces);
            });

            builder.Property(x => x.TargetDate).IsRequired();

            // MonetaryValue - CurrentAmount
            builder.OwnsOne(x => x.CurrentAmount, current =>
            {
                current.Property(c => c.Amount)
                    .HasColumnName("CurrentAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();

                current.Property(c => c.Currency)
                    .HasColumnName("CurrentCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                current.Ignore(c => c.DecimalPlaces);
            });

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description).HasMaxLength(1000);

            // Relationships
            builder.HasMany<GoalContribution>()
                .WithOne()
                .HasForeignKey(c => c.GoalId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_FinancialGoals_UserId");

            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_FinancialGoals_Type");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_FinancialGoals_Status");

            builder.HasIndex(x => x.TargetDate)
                .HasDatabaseName("IX_FinancialGoals_TargetDate");

            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_FinancialGoals_UserId_Status");

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_FinancialGoals_TenantId");
        }
    }
}
