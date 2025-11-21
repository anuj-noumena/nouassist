using System;
using System.Collections.Generic;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for data source information
    /// </summary>
    public class DataSourceDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DataSourceType Type { get; set; }
        
        public bool IsActive { get; set; }
        
        public DataConnectionStringDto ConnectionString { get; set; }
        
        public DataSourceCredentialsDto Credentials { get; set; }
        
        public List<DataTransformationStepDto> TransformationSteps { get; set; }
        
        public List<AggregatedDataViewDto> AggregatedDataViews { get; set; }
    }
}
