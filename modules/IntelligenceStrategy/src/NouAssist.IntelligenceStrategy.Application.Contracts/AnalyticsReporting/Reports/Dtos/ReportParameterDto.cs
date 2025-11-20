using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for report parameter view model
    /// </summary>
    public class ReportParameterDto : EntityDto
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Parameter data type
        /// </summary>
        public string DataType { get; set; }
        
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
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Description of the parameter
        /// </summary>
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
