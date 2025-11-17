using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Enums;
using NouAssist.WorkManagement.Approval.Model;
using NouAssist.WorkManagement.EntityFrameworkCore.Approval;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Approval.Repositories
{
    public class EfCoreApprovalAnalyticsRepository : IApprovalAnalyticsRepository, ITransientDependency
    {
        private readonly IDbContextProvider<IWorkManagementDbContext> _dbContextProvider;

        public EfCoreApprovalAnalyticsRepository(IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<Dictionary<Guid, int>> GetRequestCountByWorkflowAsync(
            DateTime startDate, 
            DateTime endDate)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            return await dbContext.ApprovalRequests
                .Where(r => r.SubmittedAt >= startDate && r.SubmittedAt <= endDate)
                .GroupBy(r => r.WorkflowId)
                .Select(g => new { WorkflowId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.WorkflowId, x => x.Count);
        }

        public async Task<Dictionary<Guid, double>> GetAverageProcessingTimeByWorkflowAsync(
            DateTime startDate, 
            DateTime endDate)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            return await dbContext.ApprovalRequests
                .Where(r => r.SubmittedAt >= startDate && 
                           r.SubmittedAt <= endDate && 
                           r.CompletedAt.HasValue)
                .GroupBy(r => r.WorkflowId)
                .Select(g => new
                {
                    WorkflowId = g.Key,
                    AvgHours = g.Average(r => (r.CompletedAt.Value - r.SubmittedAt).TotalHours)
                })
                .ToDictionaryAsync(x => x.WorkflowId, x => (double)x.AvgHours);
        }

        public async Task<Dictionary<DateTime, int>> GetRequestCountByDateAsync(
            DateTime startDate, 
            DateTime endDate)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            return await dbContext.ApprovalRequests
                .Where(r => r.SubmittedAt >= startDate && r.SubmittedAt <= endDate)
                .GroupBy(r => r.SubmittedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count);
        }

        public async Task<Dictionary<Guid, int>> GetDecisionCountByUserAsync(
            DateTime startDate, 
            DateTime endDate)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            // This would require joining with decisions
            // Simplified version - in real scenario, you'd query the decisions table
            return new Dictionary<Guid, int>();
        }

        public async Task<List<ApprovalTrend>> GetTrendDataAsync(
            DateTime startDate, 
            DateTime endDate, 
            string interval)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            // Optimized query for trend data
            var trends = await dbContext.ApprovalRequests
                .Where(r => r.SubmittedAt >= startDate && r.SubmittedAt <= endDate)
                .GroupBy(r => r.SubmittedAt.Date)
                .Select(g => new ApprovalTrend
                {
                    Date = g.Key,
                    RequestsCreated = g.Count(),
                    RequestsCompleted = g.Count(r => r.CompletedAt.HasValue),
                    RequestsApproved = g.Count(r => r.Status == ApprovalStatus.Approved),
                    RequestsRejected = g.Count(r => r.Status == ApprovalStatus.Rejected),
                    RequestsCancelled = g.Count(r => r.Status == ApprovalStatus.Cancelled),
                    PendingRequests = g.Count(r => r.Status == ApprovalStatus.Pending || 
                                                    r.Status == ApprovalStatus.InProgress)
                })
                .OrderBy(t => t.Date)
                .ToListAsync();

            return trends;
        }
    }
}
