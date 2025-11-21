using System;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when a strategic goal is defined
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoalDefined")]
    public class StrategicGoalDefinedEto : StrategicPlanningEtoBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GoalPriority Priority { get; set; }
        public PlanningHorizon Horizon { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
