namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for dashboard layout setting view model
    /// </summary>
    public class DashboardLayoutSettingDto
    {
        /// <summary>
        /// Number of columns in the layout
        /// </summary>
        public int ColumnCount { get; set; }
        
        /// <summary>
        /// Layout type (e.g., grid, masonry, fixed)
        /// </summary>
        public string LayoutType { get; set; }
        
        /// <summary>
        /// Whether automatic resizing is enabled
        /// </summary>
        public bool AutoResize { get; set; }
        
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
