using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a snapshot of user performance metrics
    /// </summary>
    public class PerformanceSnapshot : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user this snapshot belongs to
        /// </summary>
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Title of the snapshot
        /// </summary>
        public string Title { get; protected set; }
        
        /// <summary>
        /// Type of snapshot (Daily, Weekly, Monthly, etc.)
        /// </summary>
        public string SnapshotType { get; protected set; }
        
        /// <summary>
        /// Start date of the period
        /// </summary>
        public DateTime StartDate { get; protected set; }
        
        /// <summary>
        /// End date of the period
        /// </summary>
        public DateTime EndDate { get; protected set; }
        
        /// <summary>
        /// Summary text with key insights
        /// </summary>
        public string Summary { get; private set; }
        
        /// <summary>
        /// Performance metric values
        /// </summary>
        private readonly List<PerformanceMetricValue> _metricValues;
        
        /// <summary>
        /// Benchmark data for comparison
        /// </summary>
        private readonly List<BenchmarkData> _benchmarks;
        
        /// <summary>
        /// Insights derived from the performance data
        /// </summary>
        private readonly List<InsightText> _insights;
        
        /// <summary>
        /// Public access to metric values
        /// </summary>
        public IReadOnlyCollection<PerformanceMetricValue> MetricValues => _metricValues.AsReadOnly();
        
        /// <summary>
        /// Public access to benchmark data
        /// </summary>
        public IReadOnlyCollection<BenchmarkData> Benchmarks => _benchmarks.AsReadOnly();
        
        /// <summary>
        /// Public access to insights
        /// </summary>
        public IReadOnlyCollection<InsightText> Insights => _insights.AsReadOnly();

        protected PerformanceSnapshot()
        {
            _metricValues = new List<PerformanceMetricValue>();
            _benchmarks = new List<BenchmarkData>();
            _insights = new List<InsightText>();
        }

        /// <summary>
        /// Creates a new performance snapshot
        /// </summary>
        public static PerformanceSnapshot Create(
            Guid id,
            Guid userId,
            string title,
            string snapshotType,
            DateTime startDate,
            DateTime endDate,
            string summary = null,
            Guid? tenantId = null)
        {
            return new PerformanceSnapshot(id, userId, title, snapshotType, startDate, endDate, summary, tenantId);
        }

        private PerformanceSnapshot(
            Guid id,
            Guid userId,
            string title,
            string snapshotType,
            DateTime startDate,
            DateTime endDate,
            string summary,
            Guid? tenantId) : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            SnapshotType = Check.NotNullOrWhiteSpace(snapshotType, nameof(snapshotType));
            StartDate = startDate;
            EndDate = endDate;
            Summary = summary;
            
            _metricValues = new List<PerformanceMetricValue>();
            _benchmarks = new List<BenchmarkData>();
            _insights = new List<InsightText>();
        }

        /// <summary>
        /// Sets the summary text
        /// </summary>
        public void SetSummary(string summary)
        {
            Summary = summary;
        }

        /// <summary>
        /// Adds a performance metric value
        /// </summary>
        public void AddMetricValue(
            string metricName,
            double value,
            string unit = null,
            string category = null)
        {
            Check.NotNullOrWhiteSpace(metricName, nameof(metricName));
            
            _metricValues.Add(new PerformanceMetricValue(metricName, value, unit, category));
        }

        /// <summary>
        /// Adds benchmark data for comparison
        /// </summary>
        public void AddBenchmark(
            string benchmarkName,
            string benchmarkType,
            double value,
            string unit = null)
        {
            Check.NotNullOrWhiteSpace(benchmarkName, nameof(benchmarkName));
            Check.NotNullOrWhiteSpace(benchmarkType, nameof(benchmarkType));
            
            _benchmarks.Add(new BenchmarkData(benchmarkName, benchmarkType, value, unit));
        }

        /// <summary>
        /// Adds an insight derived from performance data
        /// </summary>
        public void AddInsight(
            string insightText,
            string category = null,
            double? relevanceScore = null)
        {
            Check.NotNullOrWhiteSpace(insightText, nameof(insightText));
            
            _insights.Add(new InsightText(insightText, category, relevanceScore));
        }

        /// <summary>
        /// Gets a metric value by name
        /// </summary>
        public double? GetMetricValue(string metricName)
        {
            var metric = _metricValues.Find(m => m.MetricName == metricName);
            return metric?.Value;
        }

        /// <summary>
        /// Compares user performance with benchmark
        /// </summary>
        public double? CompareToBenchmark(string metricName, string benchmarkType)
        {
            var metric = _metricValues.Find(m => m.MetricName == metricName);
            var benchmark = _benchmarks.Find(b => b.BenchmarkName == metricName && b.BenchmarkType == benchmarkType);
            
            if (metric == null || benchmark == null)
            {
                return null;
            }
            
            return metric.Value - benchmark.Value;
        }
    }
}
