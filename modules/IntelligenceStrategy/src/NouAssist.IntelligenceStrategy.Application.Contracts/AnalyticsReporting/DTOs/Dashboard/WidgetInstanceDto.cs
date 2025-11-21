using System;
using System.Collections.Generic;
using NouAssist.Analytics.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for dashboard widget information
    /// </summary>
    public class WidgetInstanceDto
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public WidgetType Type { get; set; }
        
        public Guid? DataSourceId { get; set; }
        
        public string DataSourceName { get; set; }
        
        public WidgetSettingsDto Settings { get; set; }
        
        public int PositionX { get; set; }
        
        public int PositionY { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
    }
}
