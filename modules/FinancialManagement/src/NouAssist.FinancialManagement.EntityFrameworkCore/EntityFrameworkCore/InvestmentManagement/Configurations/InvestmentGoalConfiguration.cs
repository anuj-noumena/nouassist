using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.InvestmentManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore.Configurations
{
    public class InvestmentGoalConfiguration : IEntityTypeConfiguration<InvestmentGoal>
    {
        public void Configure(EntityTypeBuilder<InvestmentGoal> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                InvestmentManagementDbProperties.DbTablePrefix + "InvestmentGoals",
                InvestmentManagementDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.TargetAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.TargetDate)
                .IsRequired();

            builder.Property(x => x.CurrentAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.ProgressPercentage)
                .HasPrecision(5, 2);

            builder.Property(x => x.RecommendedRiskLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsAchieved)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => new { x.UserId, x.IsAchieved });
            builder.HasIndex(x => x.TargetDate);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
