using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking
{
    /// <summary>
    /// Application service for goal tracking and monitoring
    /// </summary>
    public interface IGoalTrackingAppService : IApplicationService
    {
        // Goal progress
        Task<GoalProgressDto> GetGoalProgressAsync(Guid goalId);
        Task<List<GoalProgressDto>> GetGoalProgressListAsync(GetGoalsDto input);

        // Goal alerts
        Task<List<GoalAlertDto>> GetGoalAlertsAsync(GetGoalAlertsDto input);
        Task ResolveAlertAsync(Guid alertId, ResolveAlertDto input);

        // Dashboard and summaries
        Task<GoalDashboardDto> GetGoalDashboardAsync();
        Task<GoalSummaryDto> GetGoalSummaryAsync();

        // Specific queries
        Task<List<GoalProgressDto>> GetActiveGoalsAsync();
        Task<List<GoalProgressDto>> GetOverdueGoalsAsync();
        Task<List<GoalProgressDto>> GetGoalsNearingDeadlineAsync(int daysAhead = 30);
        Task<List<GoalProgressDto>> GetRecentlyAchievedGoalsAsync(int daysBack = 30);

        // Notifications
        Task<List<GoalAlertDto>> CheckAllGoalAlertsAsync();
        Task SendGoalNotificationsAsync();
    }
}
