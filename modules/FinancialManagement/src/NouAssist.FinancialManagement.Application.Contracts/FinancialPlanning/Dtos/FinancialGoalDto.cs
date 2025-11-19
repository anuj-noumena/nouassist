using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class FinancialGoalDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public GoalType Type { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal CurrentAmount { get; set; }
        public GoalStatus Status { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal ProgressPercentage { get; set; }
    }

    public class CreateFinancialGoalDto
    {
        public string Name { get; set; }
        public GoalType Type { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }

    public class UpdateFinancialGoalProgressDto
    {
        public Guid GoalId { get; set; }
        public decimal Amount { get; set; }
    }

    public class GetFinancialGoalListDto : PagedAndSortedResultRequestDto
    {
        public GoalType? TypeFilter { get; set; }
        public GoalStatus? StatusFilter { get; set; }
        public bool? ActiveOnly { get; set; } = true;
    }
}
