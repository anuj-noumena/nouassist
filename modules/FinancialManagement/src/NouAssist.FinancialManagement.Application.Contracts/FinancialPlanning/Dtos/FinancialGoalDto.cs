using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class FinancialGoalDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public GoalType Type { get; set; }
        public MonetaryValue TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public MonetaryValue CurrentAmount { get; set; }
        public decimal ProgressPercentage { get; set; }
        public GoalStatus Status { get; set; }
        public string Description { get; set; }
        public int DaysRemaining => (TargetDate - DateTime.Today).Days;
    }

    public class CreateFinancialGoalDto
    {
        public string Name { get; set; }
        public GoalType Type { get; set; }
        public decimal TargetAmount { get; set; }
        public string Currency { get; set; }
        public DateTime TargetDate { get; set; }
        public string Description { get; set; }
    }

    public class AddGoalContributionDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ContributionDate { get; set; }
        public string Notes { get; set; }
    }
}
