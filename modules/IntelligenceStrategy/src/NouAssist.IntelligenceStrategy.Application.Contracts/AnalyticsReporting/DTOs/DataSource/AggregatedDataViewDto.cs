using System;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for aggregated data view information
    /// </summary>
    public class AggregatedDataViewDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string ViewDefinition { get; set; }
        
        public string RefreshStrategy { get; set; }
        
        public DateTime LastRefreshed { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
