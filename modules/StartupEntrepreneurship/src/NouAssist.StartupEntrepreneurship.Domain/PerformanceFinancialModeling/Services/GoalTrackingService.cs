namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for goal tracking and management
    /// </summary>
    public class GoalTrackingService : DomainService, IGoalTrackingService
    {
        private readonly ITrackedMetricRepository _metricRepository;
        private readonly ILogger<GoalTrackingService> _logger;

        public GoalTrackingService(
            ITrackedMetricRepository metricRepository,
            ILogger<GoalTrackingService> logger)
        {
            _metricRepository = metricRepository;
            _logger = logger;
        }

        public async Task<List<MetricGoal>> GetActiveGoalsAsync(Guid? tenantId = null)
        {
            var metrics = await _metricRepository.GetMetricsWithActiveGoalsAsync(tenantId);
            return metrics.SelectMany(m => m.Goals.Where(g => g.IsActive && !g.Target.IsOverdue())).ToList();
        }

        public async Task<List<MetricGoal>> GetOverdueGoalsAsync(Guid? tenantId = null)
        {
            var metrics = await _metricRepository.GetMetricsWithActiveGoalsAsync(tenantId);
            return metrics.SelectMany(m => m.Goals.Where(g => g.IsActive && g.Target.IsOverdue())).ToList();
        }

        public async Task<List<MetricGoal>> GetGoalsNearingDeadlineAsync(int daysAhead = 30, Guid? tenantId = null)
        {
            var metrics = await _metricRepository.GetMetricsWithActiveGoalsAsync(tenantId);
            var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);
            
            return metrics.SelectMany(m => m.Goals.Where(g => 
                g.IsActive && 
                !g.Target.IsOverdue() && 
                g.Target.TargetDate <= cutoffDate)).ToList();
        }

        public async Task<GoalProgress> CalculateGoalProgressAsync(MetricGoal goal, TrackedMetric metric)
        {
            await Task.CompletedTask;

            var currentValue = metric.LastValue ?? new MetricValue(0, metric.DataType, metric.Unit);
            var progressPercentage = goal.GetProgress(currentValue);
            var timeProgress = goal.GetTimeProgress();
            var isOnTrack = goal.IsOnTrack(currentValue);

            // Simple projection based on current trend
            var trend = metric.GetTrend() ?? 0;
            var daysRemaining = goal.Target.GetDaysRemaining();
            var projectedValue = currentValue.Value + (currentValue.Value * trend / 100 * daysRemaining / 30);

            return new GoalProgress
            {
                GoalId = goal.Id,
                ProgressPercentage = progressPercentage,
                IsOnTrack = isOnTrack,
                DaysRemaining = daysRemaining,
                CurrentValue = currentValue,
                TargetValue = goal.Target.TargetValue,
                ProjectedValue = projectedValue,
                Status = GetGoalStatus(goal, progressPercentage, timeProgress, isOnTrack)
            };
        }

        public async Task<List<GoalAlert>> CheckGoalAlertsAsync(Guid? tenantId = null)
        {
            var alerts = new List<GoalAlert>();
            var metrics = await _metricRepository.GetMetricsWithActiveGoalsAsync(tenantId);

            foreach (var metric in metrics)
            {
                foreach (var goal in metric.Goals.Where(g => g.IsActive))
                {
                    var progress = await CalculateGoalProgressAsync(goal, metric);
                    var goalAlerts = GenerateGoalAlerts(goal, metric, progress);
                    alerts.AddRange(goalAlerts);
                }
            }

            return alerts.OrderByDescending(a => a.Priority).ToList();
        }

        public async Task<GoalSummary> GetGoalSummaryAsync(Guid? tenantId = null)
        {
            var metrics = await _metricRepository.GetActiveMetricsAsync(tenantId);
            var allGoals = metrics.SelectMany(m => m.Goals).ToList();
            var activeGoals = allGoals.Where(g => g.IsActive).ToList();
            var achievedGoals = activeGoals.Where(g => g.IsAchieved).ToList();
            var overdueGoals = activeGoals.Where(g => g.Target.IsOverdue()).ToList();

            var goalsWithProgress = new List<(MetricGoal goal, GoalProgress progress)>();
            foreach (var metric in metrics)
            {
                foreach (var goal in metric.Goals.Where(g => g.IsActive))
                {
                    var progress = await CalculateGoalProgressAsync(goal, metric);
                    goalsWithProgress.Add((goal, progress));
                }
            }

            var averageProgress = goalsWithProgress.Any() 
                ? goalsWithProgress.Average(g => g.progress.ProgressPercentage) 
                : 0;

            var goalsOnTrack = goalsWithProgress.Count(g => g.progress.IsOnTrack);
            var goalsAtRisk = goalsWithProgress.Count(g => !g.progress.IsOnTrack && !g.goal.Target.IsOverdue());

            return new GoalSummary
            {
                TotalGoals = allGoals.Count,
                ActiveGoals = activeGoals.Count,
                AchievedGoals = achievedGoals.Count,
                OverdueGoals = overdueGoals.Count,
                AverageProgress = averageProgress,
                GoalsOnTrack = goalsOnTrack,
                GoalsAtRisk = goalsAtRisk
            };
        }

        private string GetGoalStatus(MetricGoal goal, decimal progressPercentage, decimal timeProgress, bool isOnTrack)
        {
            if (goal.IsAchieved) return "Achieved";
            if (goal.Target.IsOverdue()) return "Overdue";
            if (isOnTrack) return "On Track";
            if (progressPercentage < timeProgress - 20) return "At Risk";
            if (progressPercentage < timeProgress - 10) return "Behind";
            return "On Track";
        }

        private List<GoalAlert> GenerateGoalAlerts(MetricGoal goal, TrackedMetric metric, GoalProgress progress)
        {
            var alerts = new List<GoalAlert>();

            // Overdue alert
            if (goal.Target.IsOverdue() && !goal.IsAchieved)
            {
                alerts.Add(new GoalAlert
                {
                    GoalId = goal.Id,
                    MetricName = metric.Name,
                    AlertType = "Overdue",
                    Message = $"Goal for {metric.Name} is overdue",
                    AlertDate = DateTime.UtcNow,
                    Priority = 3
                });
            }

            // At risk alert
            if (!progress.IsOnTrack && !goal.Target.IsOverdue())
            {
                alerts.Add(new GoalAlert
                {
                    GoalId = goal.Id,
                    MetricName = metric.Name,
                    AlertType = "AtRisk",
                    Message = $"Goal for {metric.Name} is at risk of not being achieved",
                    AlertDate = DateTime.UtcNow,
                    Priority = 2
                });
            }

            // Nearing deadline alert
            if (progress.DaysRemaining <= 7 && !goal.IsAchieved)
            {
                alerts.Add(new GoalAlert
                {
                    GoalId = goal.Id,
                    MetricName = metric.Name,
                    AlertType = "NearingDeadline",
                    Message = $"Goal for {metric.Name} deadline is approaching ({progress.DaysRemaining} days remaining)",
                    AlertDate = DateTime.UtcNow,
                    Priority = 2
                });
            }

            // Achievement alert
            if (goal.IsAchieved && goal.AchievedAt >= DateTime.UtcNow.AddDays(-1))
            {
                alerts.Add(new GoalAlert
                {
                    GoalId = goal.Id,
                    MetricName = metric.Name,
                    AlertType = "Achieved",
                    Message = $"Goal for {metric.Name} has been achieved!",
                    AlertDate = DateTime.UtcNow,
                    Priority = 1
                });
            }

            return alerts;
        }
    }
}
