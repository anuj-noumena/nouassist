using System;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using NouAssist.SharedKernel.Extension;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Aggregates
{
    public class InvestmentGoal : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid? PortfolioId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal TargetAmount { get; private set; }
        public DateTime TargetDate { get; private set; }
        public decimal CurrentAmount { get; private set; }
        public decimal ProgressPercentage { get; private set; }
        public RiskTolerance RecommendedRiskLevel { get; private set; }
        public bool IsAchieved { get; private set; }

        protected InvestmentGoal() { /* ORM */ }

        public InvestmentGoal(
            Guid id,
            Guid userId,
            Guid? tenantId,
            string name,
            string description,
            decimal targetAmount,
            DateTime targetDate,
            RiskTolerance recommendedRiskLevel)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            TargetAmount = Check.Positive(targetAmount, nameof(targetAmount));
            
            if (targetDate <= DateTime.UtcNow)
            {
                throw new BusinessException("InvestmentManagement:TargetDateMustBeFuture");
            }
            
            TargetDate = targetDate;
            RecommendedRiskLevel = recommendedRiskLevel;
            CurrentAmount = 0;
            ProgressPercentage = 0;
            IsAchieved = false;
        }

        public void LinkToPortfolio(Guid portfolioId)
        {
            PortfolioId = portfolioId;
        }

        public void UpdateProgress(decimal currentAmount)
        {
            CurrentAmount = Ensure.NotNegative(currentAmount, nameof(currentAmount));
            ProgressPercentage = (CurrentAmount / TargetAmount) * 100;

            if (CurrentAmount >= TargetAmount && !IsAchieved)
            {
                MarkAsAchieved();
            }
        }

        private void MarkAsAchieved()
        {
            IsAchieved = true;
        }

        public int GetMonthsRemaining()
        {
            var months = ((TargetDate.Year - DateTime.UtcNow.Year) * 12) + 
                         (TargetDate.Month - DateTime.UtcNow.Month);
            return Math.Max(0, months);
        }

        public decimal GetRequiredMonthlyContribution()
        {
            var remaining = TargetAmount - CurrentAmount;
            var months = GetMonthsRemaining();
            
            return months > 0 ? remaining / months : remaining;
        }
    }
}
