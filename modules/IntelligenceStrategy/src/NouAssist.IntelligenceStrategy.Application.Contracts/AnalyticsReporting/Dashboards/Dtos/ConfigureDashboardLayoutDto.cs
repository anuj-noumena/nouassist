using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for configuring dashboard layout
    /// </summary>
    public class ConfigureDashboardLayoutDto
    {
        /// <summary>
        /// Number of columns in the layout
        /// </summary>
        [Range(1, 12)]
        public int ColumnCount { get; set; } = 3;
        
        /// <summary>
        /// Layout type (e.g., grid, masonry, fixed)
        /// </summary>
        [Required]
        public string LayoutType { get; set; } = "grid";
        
        /// <summary>
        /// Whether automatic resizing is enabled
        /// </summary>
        public bool AutoResize { get; set; } = true;
        
        /// <summary>
        /// CSS styles for the layout
        /// </summary>
        public string CustomCss { get; set; }
        
        /// <summary>
        /// Serialized layout configuration
        /// </summary>
        public string LayoutConfig { get; set; }
    }
}
