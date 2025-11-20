using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance
{
    /// <summary>
    /// Application service for performance tracking
    /// </summary>
    public interface IPerformanceAppService : IApplicationService
    {
        /// <summary>
        /// Gets a performance snapshot by ID
        /// </summary>
        Task<PerformanceSnapshotDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets performance snapshots for a user
        /// </summary>
        Task<PagedResultDto<PerformanceSnapshotDto>> GetListAsync(GetPerformanceSnapshotsDto input);
        
        /// <summary>
        /// Gets the latest performance snapshot for a user
        /// </summary>
        Task<PerformanceSnapshotDto> GetLatestAsync(Guid userId, string snapshotType = null);
        
        /// <summary>
        /// Creates a new performance snapshot
        /// </summary>
        Task<PerformanceSnapshotDto> CreateAsync(CreatePerformanceSnapshotDto input);
        
        /// <summary>
        /// Generates performance insights
        /// </summary>
        Task<List<string>> GenerateInsightsAsync(GeneratePerformanceInsightsDto input);
        
        /// <summary>
        /// Gets performance trends for a metric
        /// </summary>
        Task<PerformanceTrendDto> GetTrendAsync(Guid userId, string metricName, DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Compares performance with benchmarks
        /// </summary>
        Task<Dictionary<string, double>> CompareWithBenchmarksAsync(Guid userId, string benchmarkType, DateTime startDate, DateTime endDate);
    }
}
