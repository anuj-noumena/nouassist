namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a new financial model is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModelCreated")]
    public class FinancialModelCreatedEto : PerformanceFinancialModelingEtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelStatus Status { get; set; }
        public int ProjectionYears { get; set; }
    }
}
