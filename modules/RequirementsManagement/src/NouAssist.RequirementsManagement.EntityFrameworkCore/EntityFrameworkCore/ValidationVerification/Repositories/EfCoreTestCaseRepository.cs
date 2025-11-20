using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Repositories
{
    public class EfCoreTestCaseRepository 
        : EfCoreRepository<IValidationVerificationDbContext, TestCase, Guid>, ITestCaseRepository 
    {
        public EfCoreTestCaseRepository(
            IDbContextProvider<IValidationVerificationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<TestCase>> GetTestCasesForRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(tc => tc.RequirementIds.Contains(requirementId))
                .Include(tc => tc.TestSteps)
                .Include(tc => tc.ExecutionLogs)
                .OrderBy(tc => tc.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCase>> GetTestCasesForRequirementsAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(tc => tc.RequirementIds.Any(id => requirementIds.Contains(id)))
                .Include(tc => tc.TestSteps)
                .Include(tc => tc.ExecutionLogs)
                .OrderBy(tc => tc.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCase>> GetTestCasesByStatusAsync(
            TestCaseStatus status,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            return await query
                .Where(tc => tc.Status == status)
                .Include(tc => tc.TestSteps)
                .OrderByDescending(tc => tc.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCase>> GetTestCasesByPriorityAsync(
            TestCasePriority priority,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            return await query
                .Where(tc => tc.Priority == priority)
                .Include(tc => tc.TestSteps)
                .OrderBy(tc => tc.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCase>> SearchTestCasesAsync(
            string searchTerm,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            return await query
                .Where(tc => tc.Name.Contains(searchTerm) || 
                            tc.Description.Contains(searchTerm))
                .Include(tc => tc.TestSteps)
                .OrderBy(tc => tc.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCase>> GetTestCasesWithExecutionsInRangeAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            return await query
                .Where(tc => tc.ExecutionLogs.Any(el => 
                    el.StartedAt >= startDate && el.StartedAt <= endDate))
                .Include(tc => tc.TestSteps)
                .Include(tc => tc.ExecutionLogs.Where(el => 
                    el.StartedAt >= startDate && el.StartedAt <= endDate))
                .OrderBy(tc => tc.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCaseExecutionSummary>> GetTestCaseExecutionSummariesAsync(
            List<Guid> testCaseIds,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            var summaries = await query
                .Where(tc => testCaseIds.Contains(tc.Id))
                .Select(tc => new TestCaseExecutionSummary
                {
                    TestCaseId = tc.Id,
                    TestCaseName = tc.Name,
                    TotalExecutions = tc.ExecutionLogs.Count(el => el.CompletedAt.HasValue),
                    PassedExecutions = tc.ExecutionLogs.Count(el => el.Status == TestExecutionStatus.Pass),
                    FailedExecutions = tc.ExecutionLogs.Count(el => el.Status == TestExecutionStatus.Fail),
                    LastExecutionStatus = tc.ExecutionLogs
                        .Where(el => el.CompletedAt.HasValue)
                        .OrderByDescending(el => el.CompletedAt)
                        .Select(el => el.Status)
                        .FirstOrDefault(),
                    LastExecutionDate = tc.ExecutionLogs
                        .Where(el => el.CompletedAt.HasValue)
                        .Max(el => el.CompletedAt),
                    AverageExecutionTime = tc.ExecutionLogs
                        .Where(el => el.CompletedAt.HasValue)
                        .Average(el => EF.Functions.DateDiffSecond(el.StartedAt, el.CompletedAt.Value))
                })
                .ToListAsync(cancellationToken);

            // Convert seconds to TimeSpan for AverageExecutionTime
            foreach (var summary in summaries)
            {
                if (summary.AverageExecutionTime > 0)
                {
                    summary.AverageExecutionTime = TimeSpan.FromSeconds(summary.AverageExecutionTime);
                }
            }

            return summaries;
        }

        public async Task<TestCaseMetrics> GetTestCaseMetricsAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            var testCases = await query
                .Include(tc => tc.ExecutionLogs)
                .ToListAsync(cancellationToken);

            var metrics = new TestCaseMetrics
            {
                TotalTestCases = testCases.Count,
                TestCasesByStatus = testCases.GroupBy(tc => tc.Status)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TestCasesByPriority = testCases.GroupBy(tc => tc.Priority)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TestCasesWithoutExecutions = testCases.Count(tc => !tc.ExecutionLogs.Any()),
                RequirementsCovered = testCases.SelectMany(tc => tc.RequirementIds).Distinct().Count()
            };

            // Calculate last execution status distribution
            var lastExecutions = testCases
                .Select(tc => tc.ExecutionLogs
                    .Where(el => el.CompletedAt.HasValue)
                    .OrderByDescending(el => el.CompletedAt)
                    .FirstOrDefault()?.Status)
                .Where(status => status.HasValue)
                .Select(status => status.Value);

            metrics.LastExecutionsByStatus = lastExecutions
                .GroupBy(status => status)
                .ToDictionary(g => g.Key, g => g.Count());

            // Calculate average execution time
            var completedExecutions = testCases
                .SelectMany(tc => tc.ExecutionLogs)
                .Where(el => el.CompletedAt.HasValue)
                .ToList();

            if (completedExecutions.Any())
            {
                var avgSeconds = completedExecutions
                    .Average(el => (el.CompletedAt.Value - el.StartedAt).TotalSeconds);
                metrics.AverageExecutionTime = avgSeconds;
            }

            return metrics;
        }

        public async Task<List<TestCase>> GetTestCasesRequiringAttentionAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            
            if (tenantId.HasValue)
            {
                query = query.Where(tc => tc.TenantId == tenantId);
            }

            return await query
                .Where(tc => tc.Status == TestCaseStatus.Blocked ||
                            (tc.Status == TestCaseStatus.Ready &&
                             tc.ExecutionLogs.Any(el => el.Status == TestExecutionStatus.Fail)) ||
                            (tc.Status == TestCaseStatus.Draft &&
                             tc.CreationTime < oneWeekAgo))
                .Include(tc => tc.TestSteps)
                .Include(tc => tc.ExecutionLogs.Where(el => el.Status == TestExecutionStatus.Fail))
                .OrderByDescending(tc => tc.LastModificationTime ?? tc.CreationTime)
                .ToListAsync(cancellationToken);
        }

        // Override to include details when needed
        public override async Task<TestCase> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(tc => tc.TestSteps)
                    .Include(tc => tc.ExecutionLogs);
            }

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TestCase), id);
            }

            return entity;
        }

        public override async Task<TestCase> FindAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(tc => tc.TestSteps)
                    .Include(tc => tc.ExecutionLogs);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
