using System.Collections.Generic;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for widget settings configuration
    /// </summary>
    public class WidgetSettingsDto
    {
        public ChartType? ChartType { get; set; }
        
        public string DataQuery { get; set; }
        
        public bool ShowLegend { get; set; }
        
        public bool ShowLabels { get; set; }
        
        public string ColorScheme { get; set; }
        
        public string DateFormat { get; set; }
        
        public string NumberFormat { get; set; }
        
        public bool AutoRefresh { get; set; }
        
        public int RefreshIntervalSeconds { get; set; }
        
        public Dictionary<string, string> AdditionalSettings { get; set; }
    }
}
