namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for widget settings
    /// </summary>
    public class WidgetSettingsDto
    {
        /// <summary>
        /// Widget title to display
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Widget subtitle to display
        /// </summary>
        public string Subtitle { get; set; }
        
        /// <summary>
        /// Widget refresh interval in seconds
        /// </summary>
        public int RefreshIntervalSeconds { get; set; }
        
        /// <summary>
        /// Widget configuration as serialized JSON
        /// </summary>
        public string Configuration { get; set; }
        
        /// <summary>
        /// Whether to show the widget title
        /// </summary>
        public bool ShowTitle { get; set; }
        
        /// <summary>
        /// Whether to allow maximizing the widget
        /// </summary>
        public bool AllowMaximize { get; set; }
    }
}
