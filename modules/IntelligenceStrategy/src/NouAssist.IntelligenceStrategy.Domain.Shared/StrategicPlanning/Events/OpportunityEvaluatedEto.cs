using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when an opportunity is evaluated
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityEvaluated")]
    public class OpportunityEvaluatedEto : StrategicPlanningEtoBase
    {
        public Guid OpportunityManagementRecordId { get; set; }
        public decimal EvaluationScore { get; set; }
        public Dictionary<string, decimal> OpportunityMetrics { get; set; }
        public DateTime EvaluationDate { get; set; }
    }
}
