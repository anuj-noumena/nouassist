using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for adding a widget to a dashboard
    /// </summary>
    public class AddWidgetDto
    {
        [Required]
        public string Title { get; set; }
        
        public WidgetType Type { get; set; }
        
        public WidgetSettingsDto Settings { get; set; }
        
        public int PositionX { get; set; }
        
        public int PositionY { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Guid? DataSourceId { get; set; }
    }
}
