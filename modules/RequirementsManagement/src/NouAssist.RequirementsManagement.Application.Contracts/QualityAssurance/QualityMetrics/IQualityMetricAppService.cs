using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics
{
    /// <summary>
    /// Application service for managing quality metrics
    /// </summary>
    public interface IQualityMetricAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new quality metric snapshot
        /// </summary>
        Task<QualityMetricDto> CreateAsync(CreateQualityMetricDto input);
        
        /// <summary>
        /// Gets a quality metric snapshot by ID
        /// </summary>
        Task<QualityMetricDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of quality metric snapshots
        /// </summary>
        Task<PagedResultDto<QualityMetricDto>> GetListAsync(GetQualityMetricListDto input);
        
        /// <summary>
        /// Gets quality metrics for a specific test plan
        /// </summary>
        Task<List<QualityMetricDto>> GetByTestPlanIdAsync(Guid testPlanId);
        
        /// <summary>
        /// Gets quality metrics for a specific release version
        /// </summary>
        Task<List<QualityMetricDto>> GetByReleaseVersionAsync(string releaseVersion);
        
        /// <summary>
        /// Gets quality metrics of a specific type
        /// </summary>
        Task<List<QualityMetricDto>> GetByMetricTypeAsync(MetricType metricType);
        
        /// <summary>
        /// Gets quality metrics within a time range
        /// </summary>
        Task<List<QualityMetricDto>> GetByTimeRangeAsync(DateTime startTime, DateTime endTime);
        
        /// <summary>
        /// Calculates and captures the test pass rate for a test plan
        /// </summary>
        Task<QualityMetricDto> CaptureTestPassRateAsync(Guid testPlanId);
        
        /// <summary>
        /// Calculates and captures the open defects count for a release
        /// </summary>
        Task<QualityMetricDto> CaptureOpenDefectsAsync(string releaseVersion);
        
        /// <summary>
        /// Gets a quality metrics summary for a release
        /// </summary>
        Task<QualityMetricSummaryDto> GetReleaseSummaryAsync(string releaseVersion);
    }
}
