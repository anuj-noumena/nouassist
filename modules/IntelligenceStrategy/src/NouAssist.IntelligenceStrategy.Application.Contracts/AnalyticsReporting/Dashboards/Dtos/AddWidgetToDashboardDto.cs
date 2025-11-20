using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for adding a widget to a dashboard
    /// </summary>
    public class AddWidgetToDashboardDto
    {
        /// <summary>
        /// Type of widget
        /// </summary>
        [Required]
        public WidgetType Type { get; set; }
        
        /// <summary>
        /// Widget title
        /// </summary>
        [Required]
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
        public int PositionX { get; set; }
        
        /// <summary>
        /// Grid position - row (Y)
        /// </summary>
        [Range(0, 100)]
        public int PositionY { get; set; }
        
        /// <summary>
        /// Width in grid units
        /// </summary>
        [Range(1, 12)]
        public int Width { get; set; }
        
        /// <summary>
        /// Height in grid units
        /// </summary>
        [Range(1, 20)]
        public int Height { get; set; }
        
        /// <summary>
        /// Widget refresh interval in seconds
        /// </summary>
        public int RefreshIntervalSeconds { get; set; } = 60;
        
        /// <summary>
        /// Widget configuration as serialized JSON
        /// </summary>
        public string Configuration { get; set; }
        
        /// <summary>
        /// Whether to show the widget title
        /// </summary>
        public bool ShowTitle { get; set; } = true;
        
        /// <summary>
        /// Whether to allow maximizing the widget
        /// </summary>
        public bool AllowMaximize { get; set; } = true;
        
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
