using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.QualityAssurance.Repositories
{
    /// <summary>
    /// Repository for quality metric snapshots
    /// </summary>
    public interface IQualityMetricSnapshotRepository : IRepository<QualityMetricSnapshot, Guid>
    {
        /// <summary>
        /// Gets metrics for a specific test plan
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets metrics for a specific release version
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetByReleaseVersionAsync(
            string releaseVersion, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets metrics of a specific type
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetByMetricTypeAsync(
            MetricType metricType, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets metrics within a time range
        /// </summary>
        Task<List<QualityMetricSnapshot>> GetByTimeRangeAsync(
            DateTime startTime, 
            DateTime endTime, 
            CancellationToken cancellationToken = default);
    }
}
