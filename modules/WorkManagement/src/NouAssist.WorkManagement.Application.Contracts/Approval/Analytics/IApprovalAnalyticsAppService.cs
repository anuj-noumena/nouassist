using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Approval.Analytics.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Approval.Analytics
{
    public interface IApprovalAnalyticsAppService : IApplicationService
    {
        /// <summary>
        /// Get overall approval analytics for a date range
        /// </summary>
        Task<ApprovalAnalyticsDto> GetAnalyticsAsync(GetApprovalAnalyticsInput input);
        
        /// <summary>
        /// Get workflow-specific statistics
        /// </summary>
        Task<List<WorkflowStatisticsDto>> GetWorkflowStatisticsAsync(GetWorkflowStatisticsInput input);
        
        /// <summary>
        /// Get approval trends over time
        /// </summary>
        Task<List<ApprovalTrendDto>> GetTrendsAsync(GetApprovalTrendsInput input);
        
        /// <summary>
        /// Get user-specific approval metrics
        /// </summary>
        Task<List<UserApprovalMetricsDto>> GetUserMetricsAsync(GetUserMetricsInput input);
        
        /// <summary>
        /// Get step performance analytics
        /// </summary>
        Task<List<StepPerformanceDto>> GetStepPerformanceAsync(GetStepPerformanceInput input);
        
        /// <summary>
        /// Get SLA compliance report
        /// </summary>
        Task<SlaComplianceReportDto> GetSlaComplianceReportAsync(GetSlaComplianceInput input);
        
        /// <summary>
        /// Get dashboard summary
        /// </summary>
        Task<ApprovalDashboardDto> GetDashboardAsync();
        
        /// <summary>
        /// Get entity-specific approval history
        /// </summary>
        Task<EntityApprovalHistoryDto> GetEntityApprovalHistoryAsync(string entityType, Guid entityId);
    }
}