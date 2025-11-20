namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a new metric is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCreated")]
    public class MetricCreatedEto : PerformanceFinancialModelingEtoBase
    {
        public string Name { get; set; }
        public MetricType Type { get; set; }
        public string Unit { get; set; }
        public Guid DefinitionId { get; set; }
    }
}
