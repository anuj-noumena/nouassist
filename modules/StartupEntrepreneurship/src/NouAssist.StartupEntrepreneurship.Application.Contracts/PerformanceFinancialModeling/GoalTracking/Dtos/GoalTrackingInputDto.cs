namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking.Dtos
{
    /// <summary>
    /// Input DTO for getting goals
    /// </summary>
    public class GetGoalsDto
    {
        public bool? IsActive { get; set; }
        public bool? IsAchieved { get; set; }
        public bool? IsOverdue { get; set; }
        public DateTime? DueBefore { get; set; }
        public DateTime? DueAfter { get; set; }
        public List<Guid> MetricIds { get; set; } = new List<Guid>();
    }

    /// <summary>
    /// Input DTO for getting goal alerts
    /// </summary>
    public class GetGoalAlertsDto
    {
        [Range(1, 90)]
        public int DaysBack { get; set; } = 7;

        public string AlertType { get; set; }
        public int? MinPriority { get; set; }
        public bool? IsResolved { get; set; }
    }

    /// <summary>
    /// Input DTO for resolving alert
    /// </summary>
    public class ResolveAlertDto
    {
        public string ResolutionNotes { get; set; }
    }
}
