using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.Shared.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Configurations
{
    public class FinancialGoalConfiguration : IEntityTypeConfiguration<FinancialGoal>
    {
        public void Configure(EntityTypeBuilder<FinancialGoal> builder)
        {
            builder.ToTable(
                FinancialManagementDbProperties.DbTablePrefix + "FinancialGoals",
                FinancialManagementDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FinancialConsts.MaxNameLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // TargetAmount value object
            builder.OwnsOne(x => x.TargetAmount, ta =>
            {
                ta.Property(m => m.Amount)
                    .HasColumnName("TargetAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ta.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // CurrentAmount value object
            builder.OwnsOne(x => x.CurrentAmount, ca =>
            {
                ca.Property(m => m.Amount)
                    .HasColumnName("CurrentAmount")
                    .HasPrecision(18, 4)
                    .IsRequired();
                
                ca.Property(m => m.Currency)
                    .HasColumnName("CurrentAmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.Property(x => x.TargetDate).IsRequired();
            
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(FinancialConsts.MaxDescriptionLength);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
