using System;
using System.Collections.Generic;

namespace NouAssist.WorkManagement.Approval.Analytics.Dtos
{
    public class ApprovalAnalyticsDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int CancelledRequests { get; set; }
        public int OverdueRequests { get; set; }
        public int SlaBreachedRequests { get; set; }
        public double ApprovalRate { get; set; }
        public double RejectionRate { get; set; }
        public double CancellationRate { get; set; }
        public double AverageProcessingTimeHours { get; set; }
        public double MedianProcessingTimeHours { get; set; }
        public TimeSpan? FastestApproval { get; set; }
        public TimeSpan? SlowestApproval { get; set; }
        public List<WorkflowStatisticsDto> WorkflowStatistics { get; set; }
        public List<ApprovalTrendDto> Trends { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class WorkflowStatisticsDto
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int CancelledRequests { get; set; }
        public double ApprovalRate { get; set; }
        public double RejectionRate { get; set; }
        public double AverageTimeToCompleteHours { get; set; }
        public int SlaBreaches { get; set; }
        public double SlaComplianceRate { get; set; }
    }

    public class ApprovalTrendDto
    {
        public DateTime Date { get; set; }
        public int RequestsCreated { get; set; }
        public int RequestsCompleted { get; set; }
        public int RequestsApproved { get; set; }
        public int RequestsRejected { get; set; }
        public int RequestsCancelled { get; set; }
        public int PendingRequests { get; set; }
        public double AverageProcessingTimeHours { get; set; }
    }

    public class UserApprovalMetricsDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int PendingApprovals { get; set; }
        public int CompletedApprovals { get; set; }
        public int ApprovalsGiven { get; set; }
        public int RejectionsGiven { get; set; }
        public double ApprovalRate { get; set; }
        public double AverageResponseTimeHours { get; set; }
        public int OverdueApprovals { get; set; }
        public DateTime? LastApprovalDate { get; set; }
        public int TotalDecisions { get; set; }
    }

    public class StepPerformanceDto
    {
        public Guid StepId { get; set; }
        public string StepName { get; set; }
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int TotalProcessed { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public double ApprovalRate { get; set; }
        public double AverageProcessingTimeHours { get; set; }
        public int TimeoutCount { get; set; }
        public List<Guid> TopApprovers { get; set; }
    }

    public class SlaComplianceReportDto
    {
        public int TotalRequestsWithSla { get; set; }
        public int SlaCompliant { get; set; }
        public int SlaBreached { get; set; }
        public double ComplianceRate { get; set; }
        public List<SlaBreachDetailDto> Breaches { get; set; }
        public List<WorkflowSlaComplianceDto> WorkflowCompliance { get; set; }
    }

    public class SlaBreachDetailDto
    {
        public Guid RequestId { get; set; }
        public string Title { get; set; }
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SlaDeadline { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan BreachDuration { get; set; }
        public string Status { get; set; }
    }

    public class WorkflowSlaComplianceDto
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int TotalWithSla { get; set; }
        public int Compliant { get; set; }
        public int Breached { get; set; }
        public double ComplianceRate { get; set; }
    }

    public class ApprovalDashboardDto
    {
        public int MyPendingApprovals { get; set; }
        public int MySubmittedRequests { get; set; }
        public int MyCompletedApprovals { get; set; }
        public int OrganizationPendingApprovals { get; set; }
        public int OrganizationOverdueApprovals { get; set; }
        public int OrganizationSlaBreaches { get; set; }
        public List<RecentApprovalActivityDto> RecentActivity { get; set; }
        public List<UpcomingDeadlineDto> UpcomingDeadlines { get; set; }
        public ApprovalAnalyticsDto Last30DaysAnalytics { get; set; }
    }

    public class RecentApprovalActivityDto
    {
        public Guid RequestId { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public string Status { get; set; }
        public Guid RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityType { get; set; } // Created, Approved, Rejected, etc.
        public Guid? ActorId { get; set; }
        public string ActorName { get; set; }
    }

    public class UpcomingDeadlineDto
    {
        public Guid RequestId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public int Priority { get; set; }
        public string CurrentStepName { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class EntityApprovalHistoryDto
    {
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public int TotalApprovals { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Cancelled { get; set; }
        public List<ApprovalHistoryItemDto> History { get; set; }
    }

    public class ApprovalHistoryItemDto
    {
        public Guid RequestId { get; set; }
        public string WorkflowName { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public TimeSpan? ProcessingTime { get; set; }
    }

    // Input DTOs
    public class GetApprovalAnalyticsInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? WorkflowId { get; set; }
        public string EntityType { get; set; }
    }

    public class GetWorkflowStatisticsInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? WorkflowId { get; set; }
    }

    public class GetApprovalTrendsInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Interval { get; set; } // Daily, Weekly, Monthly
        public Guid? WorkflowId { get; set; }
    }

    public class GetUserMetricsInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Guid> UserIds { get; set; }
    }

    public class GetStepPerformanceInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? WorkflowId { get; set; }
        public Guid? StepId { get; set; }
    }

    public class GetSlaComplianceInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? WorkflowId { get; set; }
    }
}
