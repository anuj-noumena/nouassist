using System;
using NouAssist.InvestmentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.InvestmentManagement.InvestmentGoals.Dtos
{
    public class InvestmentGoalDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? PortfolioId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal ProgressPercentage { get; set; }
        public RiskTolerance RecommendedRiskLevel { get; set; }
        public bool IsAchieved { get; set; }
        public int MonthsRemaining { get; set; }
        public decimal RequiredMonthlyContribution { get; set; }
    }

    public class CreateInvestmentGoalDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public RiskTolerance RecommendedRiskLevel { get; set; }
        public Guid? PortfolioId { get; set; }
    }

    public class UpdateInvestmentGoalDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? TargetAmount { get; set; }
        public DateTime? TargetDate { get; set; }
    }

    public class UpdateGoalProgressDto
    {
        public decimal CurrentAmount { get; set; }
    }

    public class GetInvestmentGoalListDto : PagedAndSortedResultRequestDto
    {
        public bool? OnlyActive { get; set; }
    }
}
