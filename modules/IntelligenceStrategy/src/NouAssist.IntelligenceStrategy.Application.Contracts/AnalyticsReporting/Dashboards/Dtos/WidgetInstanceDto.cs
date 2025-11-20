using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for widget instance view model
    /// </summary>
    public class WidgetInstanceDto : EntityDto<Guid>
    {
        /// <summary>
        /// Type of widget
        /// </summary>
        public WidgetType Type { get; set; }
        
        /// <summary>
        /// Widget settings
        /// </summary>
        public WidgetSettingsDto Settings { get; set; }
        
        /// <summary>
        /// Grid position - column
        /// </summary>
        public int PositionX { get; set; }
        
        /// <summary>
        /// Grid position - row
        /// </summary>
        public int PositionY { get; set; }
        
        /// <summary>
        /// Width in grid units
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// Height in grid units
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// Reference to data source ID if applicable
        /// </summary>
        public Guid? DataSourceId { get; set; }
        
        /// <summary>
        /// Data source name if applicable (for display purposes)
        /// </summary>
        public string DataSourceName { get; set; }
        
        /// <summary>
        /// Visualization configuration for chart widgets
        /// </summary>
        public VisualizationConfigurationDto VisualizationConfig { get; set; }
    }
}
