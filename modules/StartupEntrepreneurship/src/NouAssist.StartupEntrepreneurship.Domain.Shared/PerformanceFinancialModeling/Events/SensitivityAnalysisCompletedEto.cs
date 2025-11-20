namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when sensitivity analysis is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.SensitivityAnalysisCompleted")]
    public class SensitivityAnalysisCompletedEto : PerformanceFinancialModelingEtoBase
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public string AnalyzedVariable { get; set; }
        public decimal MaxImpact { get; set; }
        public decimal MinImpact { get; set; }
        public decimal Elasticity { get; set; }
    }
}
