using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for updating a widget
    /// </summary>
    public class UpdateWidgetDto
    {
        /// <summary>
        /// Widget title
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Title { get; set; }
        
        /// <summary>
        /// Widget subtitle
        /// </summary>
        [StringLength(AnalyticsReportingConsts.MaxNameLength)]
        public string Subtitle { get; set; }
        
        /// <summary>
        /// Grid position - column (X)
        /// </summary>
        [Range(0, 100)]
        public int? PositionX { get; set; }
        
        /// <summary>
        /// Grid position - row (Y)
        /// </summary>
        [Range(0, 100)]
        public int? PositionY { get; set; }
        
        /// <summary>
        /// Width in grid units
        /// </summary>
        [Range(1, 12)]
        public int? Width { get; set; }
        
        /// <summary>
        /// Height in grid units
        /// </summary>
        [Range(1, 20)]
        public int? Height { get; set; }
        
        /// <summary>
        /// Widget refresh interval in seconds
        /// </summary>
        public int? RefreshIntervalSeconds { get; set; }
        
        /// <summary>
        /// Widget configuration as serialized JSON
        /// </summary>
        public string Configuration { get; set; }
        
        /// <summary>
        /// Whether to show the widget title
        /// </summary>
        public bool? ShowTitle { get; set; }
        
        /// <summary>
        /// Whether to allow maximizing the widget
        /// </summary>
        public bool? AllowMaximize { get; set; }
        
        /// <summary>
        /// Reference to data source ID if applicable
        /// </summary>
        public Guid? DataSourceId { get; set; }
        
        /// <summary>
        /// Visualization configuration for chart widgets
        /// </summary>
        public VisualizationConfigurationDto VisualizationConfig { get; set; }
    }
}
