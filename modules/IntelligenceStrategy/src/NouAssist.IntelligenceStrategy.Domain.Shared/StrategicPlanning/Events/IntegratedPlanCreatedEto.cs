using System;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when an integrated horizon plan is created
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanCreated")]
    public class IntegratedPlanCreatedEto : StrategicPlanningEtoBase
    {
        public string PlanName { get; set; }
        public string VisionStatement { get; set; }
        public PlanningHorizon PrimaryHorizon { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
    }
}
