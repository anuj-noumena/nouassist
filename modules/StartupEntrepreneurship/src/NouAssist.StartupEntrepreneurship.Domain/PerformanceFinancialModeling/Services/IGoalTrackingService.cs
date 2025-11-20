namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for tracking and managing metric goals
    /// </summary>
    public interface IGoalTrackingService
    {
        Task<List<MetricGoal>> GetActiveGoalsAsync(Guid? tenantId = null);
        Task<List<MetricGoal>> GetOverdueGoalsAsync(Guid? tenantId = null);
        Task<List<MetricGoal>> GetGoalsNearingDeadlineAsync(int daysAhead = 30, Guid? tenantId = null);
        Task<GoalProgress> CalculateGoalProgressAsync(MetricGoal goal, TrackedMetric metric);
        Task<List<GoalAlert>> CheckGoalAlertsAsync(Guid? tenantId = null);
        Task<GoalSummary> GetGoalSummaryAsync(Guid? tenantId = null);
    }

    public class GoalProgress
    {
        public Guid GoalId { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool IsOnTrack { get; set; }
        public int DaysRemaining { get; set; }
        public MetricValue CurrentValue { get; set; }
        public MetricValue TargetValue { get; set; }
        public decimal ProjectedValue { get; set; }
        public string Status { get; set; }
    }

    public class GoalAlert
    {
        public Guid GoalId { get; set; }
        public string MetricName { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public DateTime AlertDate { get; set; }
        public int Priority { get; set; }
    }

    public class GoalSummary
    {
        public int TotalGoals { get; set; }
        public int ActiveGoals { get; set; }
        public int AchievedGoals { get; set; }
        public int OverdueGoals { get; set; }
        public decimal AverageProgress { get; set; }
        public int GoalsOnTrack { get; set; }
        public int GoalsAtRisk { get; set; }
    }
}
