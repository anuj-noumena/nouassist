namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events.Base
{
    /// <summary>
    /// Base class for Performance & Financial Modeling events
    /// </summary>
    public abstract class PerformanceFinancialModelingEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
