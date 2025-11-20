using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for data transformation step view model
    /// </summary>
    public class DataTransformationStepDto : EntityDto<Guid>
    {
        /// <summary>
        /// The transformation rule
        /// </summary>
        public TransformationRuleDto Rule { get; set; }
        
        /// <summary>
        /// Order of execution
        /// </summary>
        public int ExecutionOrder { get; set; }
        
        /// <summary>
        /// Whether this step is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// When this step was last modified
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}
