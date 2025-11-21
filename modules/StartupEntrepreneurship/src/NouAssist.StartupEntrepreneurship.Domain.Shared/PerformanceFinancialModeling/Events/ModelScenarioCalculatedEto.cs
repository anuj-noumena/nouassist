using System;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a model scenario calculation is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ModelScenarioCalculated")]
    public class ModelScenarioCalculatedEto : PerformanceFinancialModelingEtoBase
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public string ScenarioName { get; set; }
        public ScenarioType ScenarioType { get; set; }
        public int ProjectionYears { get; set; }
        public decimal FinalCashPosition { get; set; }
        public int? RunwayMonths { get; set; }
    }
}
