using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services
{
    /// <summary>
    /// Defines operations for analyzing user performance
    /// </summary>
    public interface IPerformanceAnalyzer
    {
        /// <summary>
        /// Creates a performance snapshot for a period
        /// </summary>
        Task<PerformanceSnapshot> CreatePerformanceSnapshotAsync(
            Guid userId,
            string snapshotType,
            DateTime startDate,
            DateTime endDate);
        
        /// <summary>
        /// Generates insights from performance data
        /// </summary>
        Task<List<string>> GeneratePerformanceInsightsAsync(
            Guid userId,
            DateTime startDate,
            DateTime endDate,
            int maxInsights = 5);
        
        /// <summary>
        /// Compares user performance with benchmarks
        /// </summary>
        Task<Dictionary<string, double>> CompareWithBenchmarksAsync(
            Guid userId,
            string benchmarkType,
            DateTime startDate,
            DateTime endDate);
        
        /// <summary>
        /// Identifies performance trends over time
        /// </summary>
        Task<Dictionary<string, double>> IdentifyPerformanceTrendsAsync(
            Guid userId,
            string metricName,
            DateTime startDate,
            DateTime endDate);
    }
}
