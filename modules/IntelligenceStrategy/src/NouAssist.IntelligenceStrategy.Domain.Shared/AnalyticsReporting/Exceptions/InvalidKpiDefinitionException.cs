using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions
{
    /// <summary>
    /// Exception thrown when a KPI definition is invalid
    /// </summary>
    public class InvalidKpiDefinitionException : AnalyticsDomainException
    {
        public Guid KpiDefinitionId { get; }
        
        public InvalidKpiDefinitionException(Guid kpiDefinitionId, string reason)
            : base("Analytics:ErrorMessage:InvalidKpiDefinition")
        {
            KpiDefinitionId = kpiDefinitionId;
            WithData("kpiDefinitionId", kpiDefinitionId);
            WithData("reason", reason);
        }
    }
}
