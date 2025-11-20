using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for updating a transformation step
    /// </summary>
    public class UpdateTransformationStepDto
    {
        /// <summary>
        /// Name of the transformation rule
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Source field or expression
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string SourceExpression { get; set; }
        
        /// <summary>
        /// Target field
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string TargetField { get; set; }
        
        /// <summary>
        /// Transformation expression or formula
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxTransformationRuleLength)]
        public string TransformationExpression { get; set; }
        
        /// <summary>
        /// Order in which this transformation should be applied
        /// </summary>
        public int? ExecutionOrder { get; set; }
        
        /// <summary>
        /// Whether this step is enabled
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}
