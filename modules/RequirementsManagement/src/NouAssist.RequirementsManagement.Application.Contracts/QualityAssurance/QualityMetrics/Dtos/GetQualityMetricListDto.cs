using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos
{
    /// <summary>
    /// Input for filtering and paging quality metrics list
    /// </summary>
    public class GetQualityMetricListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by metric type
        /// </summary>
        public MetricType? Type { get; set; }
        
        /// <summary>
        /// Filter by test plan ID
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Filter by release version
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// Filter by minimum timestamp
        /// </summary>
        public DateTime? FromDate { get; set; }
        
        /// <summary>
        /// Filter by maximum timestamp
        /// </summary>
        public DateTime? ToDate { get; set; }
    }
}
