using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Analytics.Dtos;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Enums;
using NouAssist.WorkManagement.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Approval.Analytics
{
    [Authorize(WorkManagementPermissions.Approval.Analytics.Default)]
    public class ApprovalAnalyticsAppService : ApplicationService, IApprovalAnalyticsAppService
    {
        private readonly IApprovalRequestRepository _requestRepository;
        private readonly IApprovalWorkflowRepository _workflowRepository;

        public ApprovalAnalyticsAppService(
            IApprovalRequestRepository requestRepository,
            IApprovalWorkflowRepository workflowRepository)
        {
            _requestRepository = requestRepository;
            _workflowRepository = workflowRepository;
        }

        public async Task<ApprovalAnalyticsDto> GetAnalyticsAsync(GetApprovalAnalyticsInput input)
        {
            var startDate = input.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = input.EndDate ?? DateTime.UtcNow;

            var requests = await GetFilteredRequestsAsync(startDate, endDate, input.WorkflowId, input.EntityType);

            var completedRequests = requests.Where(r => r.CompletedAt.HasValue).ToList();
            var processingTimes = completedRequests
                .Select(r => (r.CompletedAt.Value - r.SubmittedAt).TotalHours)
                .OrderBy(t => t)
                .ToList();

            var analytics = new ApprovalAnalyticsDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRequests = requests.Count,
                PendingRequests = requests.Count(r => r.Status == ApprovalStatus.Pending),
                InProgressRequests = requests.Count(r => r.Status == ApprovalStatus.InProgress),
                ApprovedRequests = requests.Count(r => r.Status == ApprovalStatus.Approved),
                RejectedRequests = requests.Count(r => r.Status == ApprovalStatus.Rejected),
                CancelledRequests = requests.Count(r => r.Status == ApprovalStatus.Cancelled),
                OverdueRequests = requests.Count(r => r.IsOverdue),
                SlaBreachedRequests = requests.Count(r => r.IsSlaBreached),
                ApprovalRate = requests.Count > 0 
                    ? (double)requests.Count(r => r.Status == ApprovalStatus.Approved) / requests.Count * 100 
                    : 0,
                RejectionRate = requests.Count > 0 
                    ? (double)requests.Count(r => r.Status == ApprovalStatus.Rejected) / requests.Count * 100 
                    : 0,
                CancellationRate = requests.Count > 0 
                    ? (double)requests.Count(r => r.Status == ApprovalStatus.Cancelled) / requests.Count * 100 
                    : 0,
                AverageProcessingTimeHours = processingTimes.Any() ? processingTimes.Average() : 0,
                MedianProcessingTimeHours = processingTimes.Any() 
                    ? processingTimes[processingTimes.Count / 2] 
                    : 0,
                FastestApproval = processingTimes.Any() 
                    ? TimeSpan.FromHours(processingTimes.Min()) 
                    : null,
                SlowestApproval = processingTimes.Any() 
                    ? TimeSpan.FromHours(processingTimes.Max()) 
                    : null
            };

            // Get workflow statistics
            analytics.WorkflowStatistics = await GetWorkflowStatisticsAsync(
                new GetWorkflowStatisticsInput
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    WorkflowId = input.WorkflowId
                });

            // Get trends
            analytics.Trends = await GetTrendsAsync(
                new GetApprovalTrendsInput
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Interval = "Daily",
                    WorkflowId = input.WorkflowId
                });

            return analytics;
        }

        public async Task<List<WorkflowStatisticsDto>> GetWorkflowStatisticsAsync(
            GetWorkflowStatisticsInput input)
        {
            var startDate = input.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = input.EndDate ?? DateTime.UtcNow;

            var requests = await GetFilteredRequestsAsync(startDate, endDate, input.WorkflowId, null);
            var workflows = await _workflowRepository.GetListAsync();

            var statistics = new List<WorkflowStatisticsDto>();

            foreach (var workflow in workflows)
            {
                if (input.WorkflowId.HasValue && workflow.Id != input.WorkflowId.Value)
                {
                    continue;
                }

                var workflowRequests = requests.Where(r => r.WorkflowId == workflow.Id).ToList();
                
                if (!workflowRequests.Any())
                {
                    continue;
                }

                var completedRequests = workflowRequests.Where(r => r.CompletedAt.HasValue).ToList();
                var processingTimes = completedRequests
                    .Select(r => (r.CompletedAt.Value - r.SubmittedAt).TotalHours)
                    .ToList();

                statistics.Add(new WorkflowStatisticsDto
                {
                    WorkflowId = workflow.Id,
                    WorkflowName = workflow.Name,
                    TotalRequests = workflowRequests.Count,
                    PendingRequests = workflowRequests.Count(r => r.IsPending || r.IsInProgress),
                    ApprovedRequests = workflowRequests.Count(r => r.IsApproved),
                    RejectedRequests = workflowRequests.Count(r => r.IsRejected),
                    CancelledRequests = workflowRequests.Count(r => r.IsCancelled),
                    ApprovalRate = completedRequests.Any()
                        ? (double)workflowRequests.Count(r => r.IsApproved) / completedRequests.Count * 100
                        : 0,
                    RejectionRate = completedRequests.Any()
                        ? (double)workflowRequests.Count(r => r.IsRejected) / completedRequests.Count * 100
                        : 0,
                    AverageTimeToCompleteHours = processingTimes.Any() ? processingTimes.Average() : 0,
                    SlaBreaches = workflowRequests.Count(r => r.IsSlaBreached),
                    SlaComplianceRate = workflowRequests.Count(r => r.SlaDeadline.HasValue) > 0
                        ? (double)workflowRequests.Count(r => r.SlaDeadline.HasValue && !r.IsSlaBreached) 
                          / workflowRequests.Count(r => r.SlaDeadline.HasValue) * 100
                        : 100
                });
            }

            return statistics.OrderByDescending(s => s.TotalRequests).ToList();
        }

        public async Task<List<ApprovalTrendDto>> GetTrendsAsync(GetApprovalTrendsInput input)
        {
            var requests = await GetFilteredRequestsAsync(
                input.StartDate, 
                input.EndDate, 
                input.WorkflowId, 
                null);

            var trends = new List<ApprovalTrendDto>();
            var currentDate = input.StartDate.Date;

            while (currentDate <= input.EndDate.Date)
            {
                var nextDate = GetNextDate(currentDate, input.Interval);
                
                var periodRequests = requests.Where(r => 
                    r.SubmittedAt >= currentDate && r.SubmittedAt < nextDate).ToList();

                var completedInPeriod = periodRequests.Where(r => 
                    r.CompletedAt.HasValue && 
                    r.CompletedAt.Value >= currentDate && 
                    r.CompletedAt.Value < nextDate).ToList();

                var processingTimes = completedInPeriod
                    .Select(r => (r.CompletedAt.Value - r.SubmittedAt).TotalHours)
                    .ToList();

                trends.Add(new ApprovalTrendDto
                {
                    Date = currentDate,
                    RequestsCreated = periodRequests.Count,
                    RequestsCompleted = completedInPeriod.Count,
                    RequestsApproved = completedInPeriod.Count(r => r.IsApproved),
                    RequestsRejected = completedInPeriod.Count(r => r.IsRejected),
                    RequestsCancelled = completedInPeriod.Count(r => r.IsCancelled),
                    PendingRequests = periodRequests.Count(r => r.IsPending || r.IsInProgress),
                    AverageProcessingTimeHours = processingTimes.Any() ? processingTimes.Average() : 0
                });

                currentDate = nextDate;
            }

            return trends;
        }

        public async Task<List<UserApprovalMetricsDto>> GetUserMetricsAsync(GetUserMetricsInput input)
        {
            var startDate = input.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = input.EndDate ?? DateTime.UtcNow;

            var requests = await GetFilteredRequestsAsync(startDate, endDate, null, null);
            var metrics = new List<UserApprovalMetricsDto>();

            // Group by users who made decisions
            var userDecisions = requests
                .SelectMany(r => r.Decisions)
                .Where(d => d.DecidedAt >= startDate && d.DecidedAt <= endDate)
                .GroupBy(d => d.ApproverId);

            foreach (var userGroup in userDecisions)
            {
                var userId = userGroup.Key;
                
                if (input.UserIds != null && input.UserIds.Any() && !input.UserIds.Contains(userId))
                {
                    continue;
                }

                var decisions = userGroup.ToList();
                var responseTimes = decisions
                    .Select(d =>
                    {
                        var request = requests.First(r => r.Id == d.RequestId);
                        return (d.DecidedAt - request.SubmittedAt).TotalHours;
                    })
                    .ToList();

                var userRequests = requests.Where(r => 
                    r.Decisions.Any(d => d.ApproverId == userId)).ToList();

                metrics.Add(new UserApprovalMetricsDto
                {
                    UserId = userId,
                    UserName = $"User-{userId}", // Load from identity service in real scenario
                    PendingApprovals = userRequests.Count(r => !r.IsComplete),
                    CompletedApprovals = decisions.Count,
                    ApprovalsGiven = decisions.Count(d => d.Action == ApprovalAction.Approved),
                    RejectionsGiven = decisions.Count(d => d.Action == ApprovalAction.Rejected),
                    ApprovalRate = decisions.Any()
                        ? (double)decisions.Count(d => d.Action == ApprovalAction.Approved) / decisions.Count * 100
                        : 0,
                    AverageResponseTimeHours = responseTimes.Any() ? responseTimes.Average() : 0,
                    OverdueApprovals = userRequests.Count(r => r.IsOverdue),
                    LastApprovalDate = decisions.Any() ? decisions.Max(d => d.DecidedAt) : (DateTime?)null,
                    TotalDecisions = decisions.Count
                });
            }

            return metrics.OrderByDescending(m => m.TotalDecisions).ToList();
        }

        public async Task<List<StepPerformanceDto>> GetStepPerformanceAsync(GetStepPerformanceInput input)
        {
            var startDate = input.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = input.EndDate ?? DateTime.UtcNow;

            var requests = await GetFilteredRequestsAsync(startDate, endDate, input.WorkflowId, null);
            var workflows = await _workflowRepository.GetListAsync();

            var stepPerformance = new List<StepPerformanceDto>();

            foreach (var workflow in workflows)
            {
                if (input.WorkflowId.HasValue && workflow.Id != input.WorkflowId.Value)
                {
                    continue;
                }

                var workflowRequests = requests.Where(r => r.WorkflowId == workflow.Id).ToList();
                
                foreach (var step in workflow.Steps)
                {
                    if (input.StepId.HasValue && step.Id != input.StepId.Value)
                    {
                        continue;
                    }

                    var stepDecisions = workflowRequests
                        .SelectMany(r => r.Decisions)
                        .Where(d => d.StepId == step.Id)
                        .ToList();

                    if (!stepDecisions.Any())
                    {
                        continue;
                    }

                    var processingTimes = stepDecisions
                        .Select(d =>
                        {
                            var request = workflowRequests.First(r => r.Id == d.RequestId);
                            return (d.DecidedAt - request.SubmittedAt).TotalHours;
                        })
                        .ToList();

                    stepPerformance.Add(new StepPerformanceDto
                    {
                        StepId = step.Id,
                        StepName = step.Name,
                        WorkflowId = workflow.Id,
                        WorkflowName = workflow.Name,
                        TotalProcessed = stepDecisions.Count,
                        Approved = stepDecisions.Count(d => d.Action == ApprovalAction.Approved),
                        Rejected = stepDecisions.Count(d => d.Action == ApprovalAction.Rejected),
                        ApprovalRate = (double)stepDecisions.Count(d => d.Action == ApprovalAction.Approved) 
                            / stepDecisions.Count * 100,
                        AverageProcessingTimeHours = processingTimes.Average(),
                        TimeoutCount = 0, // Would need additional tracking
                        TopApprovers = stepDecisions
                            .GroupBy(d => d.ApproverId)
                            .OrderByDescending(g => g.Count())
                            .Take(5)
                            .Select(g => g.Key)
                            .ToList()
                    });
                }
            }

            return stepPerformance.OrderByDescending(s => s.TotalProcessed).ToList();
        }

        public async Task<SlaComplianceReportDto> GetSlaComplianceReportAsync(GetSlaComplianceInput input)
        {
            var startDate = input.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = input.EndDate ?? DateTime.UtcNow;

            var requests = await GetFilteredRequestsAsync(startDate, endDate, input.WorkflowId, null);
            var requestsWithSla = requests.Where(r => r.SlaDeadline.HasValue).ToList();

            var breaches = requestsWithSla
                .Where(r => r.IsSlaBreached || (r.CompletedAt.HasValue && r.CompletedAt.Value > r.SlaDeadline.Value))
                .Select(r => new SlaBreachDetailDto
                {
                    RequestId = r.Id,
                    Title = r.Title,
                    WorkflowId = r.WorkflowId,
                    WorkflowName = "", // Load from workflow
                    SubmittedAt = r.SubmittedAt,
                    SlaDeadline = r.SlaDeadline.Value,
                    CompletedAt = r.CompletedAt,
                    BreachDuration = r.CompletedAt.HasValue 
                        ? r.CompletedAt.Value - r.SlaDeadline.Value
                        : DateTime.UtcNow - r.SlaDeadline.Value,
                    Status = r.Status.ToString()
                })
                .ToList();

            var workflows = await _workflowRepository.GetListAsync();
            var workflowCompliance = new List<WorkflowSlaComplianceDto>();

            foreach (var workflow in workflows)
            {
                var workflowRequestsWithSla = requestsWithSla.Where(r => r.WorkflowId == workflow.Id).ToList();
                
                if (!workflowRequestsWithSla.Any())
                {
                    continue;
                }

                var workflowBreaches = workflowRequestsWithSla.Count(r => 
                    r.IsSlaBreached || (r.CompletedAt.HasValue && r.CompletedAt.Value > r.SlaDeadline.Value));

                workflowCompliance.Add(new WorkflowSlaComplianceDto
                {
                    WorkflowId = workflow.Id,
                    WorkflowName = workflow.Name,
                    TotalWithSla = workflowRequestsWithSla.Count,
                    Compliant = workflowRequestsWithSla.Count - workflowBreaches,
                    Breached = workflowBreaches,
                    ComplianceRate = (double)(workflowRequestsWithSla.Count - workflowBreaches) 
                        / workflowRequestsWithSla.Count * 100
                });
            }

            return new SlaComplianceReportDto
            {
                TotalRequestsWithSla = requestsWithSla.Count,
                SlaCompliant = requestsWithSla.Count - breaches.Count,
                SlaBreached = breaches.Count,
                ComplianceRate = requestsWithSla.Any()
                    ? (double)(requestsWithSla.Count - breaches.Count) / requestsWithSla.Count * 100
                    : 100,
                Breaches = breaches.OrderByDescending(b => b.BreachDuration).ToList(),
                WorkflowCompliance = workflowCompliance.OrderBy(w => w.ComplianceRate).ToList()
            };
        }

        public async Task<ApprovalDashboardDto> GetDashboardAsync()
        {
            var userId = CurrentUser.Id.Value;
            var last30Days = DateTime.UtcNow.AddDays(-30);

            var allRequests = await _requestRepository.GetListAsync();
            var myRequests = allRequests.Where(r => r.RequestedBy == userId).ToList();
            var myApprovals = allRequests
                .Where(r => r.Decisions.Any(d => d.ApproverId == userId))
                .ToList();

            var recentActivity = allRequests
                .Where(r => r.CreationTime >= last30Days)
                .OrderByDescending(r => r.CreationTime)
                .Take(10)
                .Select(r => new RecentApprovalActivityDto
                {
                    RequestId = r.Id,
                    Title = r.Title,
                    EntityType = r.EntityType,
                    Status = r.Status.ToString(),
                    RequestedBy = r.RequestedBy,
                    RequestedByName = $"User-{r.RequestedBy}",
                    ActivityDate = r.CreationTime,
                    ActivityType = "Created"
                })
                .ToList();

            var upcomingDeadlines = allRequests
                .Where(r => r.DueDate.HasValue && !r.IsComplete)
                .OrderBy(r => r.DueDate)
                .Take(10)
                .Select(r => new UpcomingDeadlineDto
                {
                    RequestId = r.Id,
                    Title = r.Title,
                    DueDate = r.DueDate.Value,
                    TimeRemaining = r.TimeRemaining ?? TimeSpan.Zero,
                    Priority = r.Priority,
                    CurrentStepName = "", // Load from workflow
                    IsOverdue = r.IsOverdue
                })
                .ToList();

            return new ApprovalDashboardDto
            {
                MyPendingApprovals = myApprovals.Count(r => !r.IsComplete),
                MySubmittedRequests = myRequests.Count,
                MyCompletedApprovals = myApprovals.Count(r => r.IsComplete),
                OrganizationPendingApprovals = allRequests.Count(r => r.IsPending || r.IsInProgress),
                OrganizationOverdueApprovals = allRequests.Count(r => r.IsOverdue),
                OrganizationSlaBreaches = allRequests.Count(r => r.IsSlaBreached),
                RecentActivity = recentActivity,
                UpcomingDeadlines = upcomingDeadlines,
                Last30DaysAnalytics = await GetAnalyticsAsync(new GetApprovalAnalyticsInput
                {
                    StartDate = last30Days,
                    EndDate = DateTime.UtcNow
                })
            };
        }

        public async Task<EntityApprovalHistoryDto> GetEntityApprovalHistoryAsync(
            string entityType, 
            Guid entityId)
        {
            var requests = await _requestRepository.GetRequestsByEntityAsync(entityType, entityId);

            return new EntityApprovalHistoryDto
            {
                EntityType = entityType,
                EntityId = entityId,
                TotalApprovals = requests.Count,
                Approved = requests.Count(r => r.IsApproved),
                Rejected = requests.Count(r => r.IsRejected),
                Cancelled = requests.Count(r => r.IsCancelled),
                History = requests
                    .OrderByDescending(r => r.SubmittedAt)
                    .Select(r => new ApprovalHistoryItemDto
                    {
                        RequestId = r.Id,
                        WorkflowName = "", // Load from workflow
                        Status = r.Status.ToString(),
                        SubmittedAt = r.SubmittedAt,
                        CompletedAt = r.CompletedAt,
                        RequestedBy = r.RequestedBy,
                        RequestedByName = $"User-{r.RequestedBy}",
                        ProcessingTime = r.CompletedAt.HasValue 
                            ? r.CompletedAt.Value - r.SubmittedAt 
                            : null
                    })
                    .ToList()
            };
        }

        private async Task<List<ApprovalRequest>> GetFilteredRequestsAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? workflowId,
            string entityType)
        {
            var allRequests = await _requestRepository.GetListAsync();

            return allRequests
                .Where(r => r.SubmittedAt >= startDate && r.SubmittedAt <= endDate)
                .Where(r => !workflowId.HasValue || r.WorkflowId == workflowId.Value)
                .Where(r => string.IsNullOrEmpty(entityType) || r.EntityType == entityType)
                .ToList();
        }

        private DateTime GetNextDate(DateTime currentDate, string interval)
        {
            return interval.ToLower() switch
            {
                "daily" => currentDate.AddDays(1),
                "weekly" => currentDate.AddDays(7),
                "monthly" => currentDate.AddMonths(1),
                _ => currentDate.AddDays(1)
            };
        }
    }
}