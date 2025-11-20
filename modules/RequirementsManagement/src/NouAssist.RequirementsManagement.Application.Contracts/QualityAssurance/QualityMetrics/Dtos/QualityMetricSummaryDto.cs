using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos
{
    /// <summary>
    /// Summary of quality metrics for a release or test plan
    /// </summary>
    public class QualityMetricSummaryDto
    {
        /// <summary>
        /// Release version associated with this summary
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// Test pass rate (percentage)
        /// </summary>
        public decimal TestPassRate { get; set; }
        
        /// <summary>
        /// Defect density (defects per 1000 lines of code)
        /// </summary>
        public decimal DefectDensity { get; set; }
        
        /// <summary>
        /// Number of open defects
        /// </summary>
        public int OpenDefectsCount { get; set; }
        
        /// <summary>
        /// Number of critical defects
        /// </summary>
        public int CriticalDefectsCount { get; set; }
        
        /// <summary>
        /// List of test pass rate metrics over time
        /// </summary>
        public List<QualityMetricDto> TestPassRateHistory { get; set; }
        
        /// <summary>
        /// List of open defects metrics over time
        /// </summary>
        public List<QualityMetricDto> OpenDefectsHistory { get; set; }
    }
}
