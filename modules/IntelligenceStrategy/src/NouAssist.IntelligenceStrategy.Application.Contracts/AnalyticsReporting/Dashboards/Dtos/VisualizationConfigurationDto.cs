using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for visualization configuration
    /// </summary>
    public class VisualizationConfigurationDto
    {
        /// <summary>
        /// Type of chart for visualization
        /// </summary>
        public ChartType ChartType { get; set; }
        
        /// <summary>
        /// JSON configuration for the chart
        /// </summary>
        public string ChartConfig { get; set; }
        
        /// <summary>
        /// The field name to use for X-axis
        /// </summary>
        public string XAxisField { get; set; }
        
        /// <summary>
        /// The field name(s) to use for Y-axis
        /// </summary>
        public List<string> YAxisFields { get; set; }
        
        /// <summary>
        /// Whether to show legends
        /// </summary>
        public bool ShowLegend { get; set; }
        
        /// <summary>
        /// Custom colors to use for the visualization
        /// </summary>
        public List<string> CustomColors { get; set; }
    }
}
