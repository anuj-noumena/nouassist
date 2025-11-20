namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for dashboard layout settings
    /// </summary>
    public class DashboardLayoutSettingDto
    {
        public int Columns { get; set; }
        
        public string Theme { get; set; }
        
        public bool AutoRefresh { get; set; }
        
        public int RefreshIntervalSeconds { get; set; }
        
        public string CustomCss { get; set; }
        
        public bool CompactView { get; set; }
    }
}
