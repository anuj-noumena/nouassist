using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for updating a parameter on a report
    /// </summary>
    public class UpdateReportParameterDto
    {
        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; set; }
        
        /// <summary>
        /// Whether the parameter is required
        /// </summary>
        public bool IsRequired { get; set; }
        
        /// <summary>
        /// Display name for the parameter
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Description of the parameter
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Order for display
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Validation rules as JSON
        /// </summary>
        public string ValidationRules { get; set; }
    }
}
