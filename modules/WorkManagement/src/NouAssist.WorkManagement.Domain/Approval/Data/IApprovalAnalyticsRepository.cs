using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NouAssist.WorkManagement.Approval.Data
{
    /// <summary>
    /// Optimized repository methods for analytics queries
    /// </summary>
    public interface IApprovalAnalyticsRepository
    {
        Task<Dictionary<Guid, int>> GetRequestCountByWorkflowAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<Guid, double>> GetAverageProcessingTimeByWorkflowAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<DateTime, int>> GetRequestCountByDateAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<Guid, int>> GetDecisionCountByUserAsync(DateTime startDate, DateTime endDate);
        Task<List<ApprovalTrendDto>> GetTrendDataAsync(DateTime startDate, DateTime endDate, string interval);
    }
}