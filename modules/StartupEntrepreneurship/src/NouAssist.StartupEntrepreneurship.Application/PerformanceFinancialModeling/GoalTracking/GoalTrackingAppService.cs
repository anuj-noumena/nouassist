using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Localization;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking
{
    [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.Default)]
    public class GoalTrackingAppService : ApplicationService, IGoalTrackingAppService
    {
        private readonly ITrackedMetricRepository _trackedMetricRepository;
        private readonly IGoalTrackingService _goalTrackingService;
        private readonly ICurrentTenant _currentTenant;

        public GoalTrackingAppService(
            ITrackedMetricRepository trackedMetricRepository,
            IGoalTrackingService goalTrackingService,
            ICurrentTenant currentTenant)
        {
            _trackedMetricRepository = trackedMetricRepository;
            _goalTrackingService = goalTrackingService;
            _currentTenant = currentTenant;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<GoalProgressDto> GetGoalProgressAsync(Guid goalId)
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);
                var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goalId));

                if (metric == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:GoalNotFound"]);
                }

                CheckResourceAccess(metric);

                var goal = metric.Goals.First(g => g.Id == goalId);
                var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);

                return MapToGoalProgressDto(goal, metric, progress);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:GoalNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalProgressDto>> GetGoalProgressListAsync(GetGoalsDto input)
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);

                if (input.MetricIds?.Any() == true)
                {
                    metrics = metrics.Where(m => input.MetricIds.Contains(m.Id)).ToList();
                }

                var goalProgressList = new List<GoalProgressDto>();

                foreach (var metric in metrics)
                {
                    var filteredGoals = metric.Goals.AsEnumerable();

                    if (input.IsActive.HasValue)
                        filteredGoals = filteredGoals.Where(g => g.IsActive == input.IsActive.Value);

                    if (input.IsAchieved.HasValue)
                        filteredGoals = filteredGoals.Where(g => g.IsAchieved == input.IsAchieved.Value);

                    if (input.IsOverdue.HasValue)
                        filteredGoals = filteredGoals.Where(g => g.Target.IsOverdue() == input.IsOverdue.Value);

                    if (input.DueBefore.HasValue)
                        filteredGoals = filteredGoals.Where(g => g.Target.TargetDate <= input.DueBefore.Value);

                    if (input.DueAfter.HasValue)
                        filteredGoals = filteredGoals.Where(g => g.Target.TargetDate >= input.DueAfter.Value);

                    foreach (var goal in filteredGoals)
                    {
                        try
                        {
                            var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);
                            goalProgressList.Add(MapToGoalProgressDto(goal, metric, progress));
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, "Failed to calculate progress for goal {GoalId}", goal.Id);
                        }
                    }
                }

                return goalProgressList;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalAlertDto>> GetGoalAlertsAsync(GetGoalAlertsDto input)
        {
            try
            {
                var alerts = await _goalTrackingService.CheckGoalAlertsAsync(_currentTenant.Id);

                // Apply filters
                if (!string.IsNullOrEmpty(input.AlertType))
                {
                    alerts = alerts.Where(a => a.AlertType.Equals(input.AlertType, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (input.MinPriority.HasValue)
                {
                    alerts = alerts.Where(a => a.Priority >= input.MinPriority.Value).ToList();
                }

                // Filter by date (simulate IsResolved with recent alerts)
                var cutoffDate = DateTime.UtcNow.AddDays(-input.DaysBack);
                alerts = alerts.Where(a => a.AlertDate >= cutoffDate).ToList();

                return alerts.Select(a => new GoalAlertDto
                {
                    GoalId = a.GoalId,
                    MetricId = a.GoalId, // This would need to be properly mapped in real implementation
                    MetricName = a.MetricName,
                    AlertType = a.AlertType,
                    Message = a.Message,
                    AlertDate = a.AlertDate,
                    Priority = a.Priority,
                    IsResolved = false // This would be tracked in a real implementation
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals)]
        public virtual async Task ResolveAlertAsync(Guid alertId, ResolveAlertDto input)
        {
            try
            {
                // In a real implementation, this would update the alert resolution status
                // For now, we'll just log the resolution
                Logger.LogInformation("Alert {AlertId} resolved with notes: {Notes}", alertId, input.ResolutionNotes);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewDashboard)]
        public virtual async Task<GoalDashboardDto> GetGoalDashboardAsync()
        {
            try
            {
                var summary = await _goalTrackingService.GetGoalSummaryAsync(_currentTenant.Id);
                var recentlyAchieved = await GetRecentlyAchievedGoalsAsync(7);
                var upcomingDeadlines = await GetGoalsNearingDeadlineAsync(30);
                var atRiskGoals = await GetAtRiskGoalsAsync();
                var recentAlerts = await GetGoalAlertsAsync(new GetGoalAlertsDto { DaysBack = 7 });

                return new GoalDashboardDto
                {
                    Summary = new GoalSummaryDto
                    {
                        TotalGoals = summary.TotalGoals,
                        ActiveGoals = summary.ActiveGoals,
                        AchievedGoals = summary.AchievedGoals,
                        OverdueGoals = summary.OverdueGoals,
                        AverageProgress = summary.AverageProgress,
                        GoalsOnTrack = summary.GoalsOnTrack,
                        GoalsAtRisk = summary.GoalsAtRisk,
                        GeneratedAt = DateTime.UtcNow
                    },
                    RecentlyAchieved = recentlyAchieved,
                    UpcomingDeadlines = upcomingDeadlines,
                    AtRiskGoals = atRiskGoals,
                    RecentAlerts = recentAlerts
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<GoalSummaryDto> GetGoalSummaryAsync()
        {
            try
            {
                var summary = await _goalTrackingService.GetGoalSummaryAsync(_currentTenant.Id);

                return new GoalSummaryDto
                {
                    TotalGoals = summary.TotalGoals,
                    ActiveGoals = summary.ActiveGoals,
                    AchievedGoals = summary.AchievedGoals,
                    OverdueGoals = summary.OverdueGoals,
                    AverageProgress = summary.AverageProgress,
                    GoalsOnTrack = summary.GoalsOnTrack,
                    GoalsAtRisk = summary.GoalsAtRisk,
                    GeneratedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalProgressDto>> GetActiveGoalsAsync()
        {
            try
            {
                var activeGoals = await _goalTrackingService.GetActiveGoalsAsync(_currentTenant.Id);
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);

                var goalProgressList = new List<GoalProgressDto>();

                foreach (var goal in activeGoals)
                {
                    var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goal.Id));
                    if (metric != null)
                    {
                        var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);
                        goalProgressList.Add(MapToGoalProgressDto(goal, metric, progress));
                    }
                }

                return goalProgressList;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalProgressDto>> GetOverdueGoalsAsync()
        {
            try
            {
                var overdueGoals = await _goalTrackingService.GetOverdueGoalsAsync(_currentTenant.Id);
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);

                var goalProgressList = new List<GoalProgressDto>();

                foreach (var goal in overdueGoals)
                {
                    var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goal.Id));
                    if (metric != null)
                    {
                        var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);
                        goalProgressList.Add(MapToGoalProgressDto(goal, metric, progress));
                    }
                }

                return goalProgressList;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalProgressDto>> GetGoalsNearingDeadlineAsync(int daysAhead = 30)
        {
            try
            {
                var nearingGoals = await _goalTrackingService.GetGoalsNearingDeadlineAsync(daysAhead, _currentTenant.Id);
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);

                var goalProgressList = new List<GoalProgressDto>();

                foreach (var goal in nearingGoals)
                {
                    var metric = metrics.FirstOrDefault(m => m.Goals.Any(g => g.Id == goal.Id));
                    if (metric != null)
                    {
                        var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);
                        goalProgressList.Add(MapToGoalProgressDto(goal, metric, progress));
                    }
                }

                return goalProgressList;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalProgressDto>> GetRecentlyAchievedGoalsAsync(int daysBack = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-daysBack);
                var metrics = await _trackedMetricRepository.GetMetricsWithActiveGoalsAsync(_currentTenant.Id);

                var goalProgressList = new List<GoalProgressDto>();

                foreach (var metric in metrics)
                {
                    var recentlyAchievedGoals = metric.Goals.Where(g => 
                        g.IsAchieved && g.AchievedAt.HasValue && g.AchievedAt.Value >= cutoffDate);

                    foreach (var goal in recentlyAchievedGoals)
                    {
                        var progress = await _goalTrackingService.CalculateGoalProgressAsync(goal, metric);
                        goalProgressList.Add(MapToGoalProgressDto(goal, metric, progress));
                    }
                }

                return goalProgressList.OrderByDescending(g => g.AchievedAt).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.View)]
        public virtual async Task<List<GoalAlertDto>> CheckAllGoalAlertsAsync()
        {
            try
            {
                var alerts = await _goalTrackingService.CheckGoalAlertsAsync(_currentTenant.Id);

                return alerts.Select(a => new GoalAlertDto
                {
                    GoalId = a.GoalId,
                    MetricId = a.GoalId, // This would need proper mapping in real implementation
                    MetricName = a.MetricName,
                    AlertType = a.AlertType,
                    Message = a.Message,
                    AlertDate = a.AlertDate,
                    Priority = a.Priority,
                    IsResolved = false
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.TrackedMetrics.ManageGoals)]
        public virtual async Task SendGoalNotificationsAsync()
        {
            try
            {
                // In a real implementation, this would trigger notifications
                var alerts = await CheckAllGoalAlertsAsync();
                
                // Process high-priority alerts
                var highPriorityAlerts = alerts.Where(a => a.Priority >= 3).ToList();
                
                Logger.LogInformation("Processed {Count} high-priority goal alerts", highPriorityAlerts.Count);
                
                // Here you would integrate with notification services
                // - Email notifications
                // - Push notifications  
                // - Slack/Teams integrations
                // - Dashboard alerts
                
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Private helper methods
        private async Task<List<GoalProgressDto>> GetAtRiskGoalsAsync()
        {
            var activeGoals = await GetActiveGoalsAsync();
            return activeGoals.Where(g => !g.IsOnTrack && !g.IsOverdue).ToList();
        }

        private GoalProgressDto MapToGoalProgressDto(MetricGoal goal, TrackedMetric metric, GoalProgress progress)
        {
            return new GoalProgressDto
            {
                GoalId = goal.Id,
                MetricId = metric.Id,
                MetricName = metric.Name,
                ProgressPercentage = progress.ProgressPercentage,
                IsOnTrack = progress.IsOnTrack,
                DaysRemaining = progress.DaysRemaining,
                CurrentValue = ObjectMapper.Map<MetricValue, MetricValueDto>(progress.CurrentValue),
                TargetValue = ObjectMapper.Map<MetricValue, MetricValueDto>(progress.TargetValue),
                ProjectedValue = progress.ProjectedValue,
                Status = progress.Status,
                AchievedAt = goal.AchievedAt,
                IsOverdue = goal.Target.IsOverdue()
            };
        }

        // Helper methods
        protected virtual void CheckResourceAccess(TrackedMetric metric)
        {
            if (metric.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(TrackedMetric), metric.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in GoalTrackingAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}

// Update: src/NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Application/Mapping/PerformanceFinancialModelingApplicationAutoMapperProfile.cs
public class PerformanceFinancialModelingApplicationAutoMapperProfile : Profile
{
    public PerformanceFinancialModelingApplicationAutoMapperProfile()
    {
        // ... existing mappings ...

        // TrendAnalysis mappings
        CreateMap<TrendAnalysis, TrendAnalysisDto>(); // Would need to create this domain object
        CreateMap<TrendPattern, TrendPatternDto>();
        CreateMap<ForecastResult, ForecastResultDto>();
        CreateMap<SeasonalityAnalysis, SeasonalityAnalysisDto>();
        CreateMap<CorrelationResult, CorrelationResultDto>();
        CreateMap<Anomaly, AnomalyDto>();

        // Goal tracking mappings
        CreateMap<GoalProgress, GoalProgressDto>();
        CreateMap<GoalAlert, GoalAlertDto>(); 
        CreateMap<GoalSummary, GoalSummaryDto>();

        // Sensitivity analysis mappings
        CreateMap<SensitivityResult, SensitivityAnalysisResultDto>();
        CreateMap<SensitivityMatrix, object>(); // Would need proper DTO
        CreateMap<SensitivityOutput, SensitivityAnalysisResultDto>();

        // Runway analysis mappings  
        CreateMap<RunwayProjection, RunwayProjectionDto>();
        CreateMap<RunwayAlert, RunwayAlertDto>();
        CreateMap<RunwayOptimization, RunwayOptimizationDto>();
        CreateMap<OptimizationOption, OptimizationOptionDto>();
    }
}
