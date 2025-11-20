using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for filtering Strategic Goal lists
    /// </summary>
    public class GetStrategicGoalListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public GoalStatus? Status { get; set; }
        public GoalPriority? Priority { get; set; }
        public PlanningHorizon? Horizon { get; set; }
        public Guid? OwnerId { get; set; }
        public bool? IsOnTrack { get; set; }
    }
}
