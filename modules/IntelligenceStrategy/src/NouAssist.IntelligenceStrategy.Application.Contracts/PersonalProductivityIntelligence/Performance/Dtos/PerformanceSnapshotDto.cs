using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for performance snapshot
    /// </summary>
    public class PerformanceSnapshotDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Title of the snapshot
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Type of snapshot (Daily, Weekly, Monthly, etc.)
        /// </summary>
        public string SnapshotType { get; set; }
        
        /// <summary>
        /// Start date of the period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Summary text
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// Metric values
        /// </summary>
        public List<PerformanceMetricDto> MetricValues { get; set; }
        
        /// <summary>
        /// Benchmark data
        /// </summary>
        public List<BenchmarkDataDto> Benchmarks { get; set; }
        
        /// <summary>
        /// Insights
        /// </summary>
        public List<InsightTextDto> Insights { get; set; }
    }
}
