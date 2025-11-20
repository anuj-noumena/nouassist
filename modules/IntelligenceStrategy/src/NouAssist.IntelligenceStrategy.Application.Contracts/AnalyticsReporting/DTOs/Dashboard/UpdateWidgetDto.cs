using System.ComponentModel.DataAnnotations;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for updating a dashboard widget
    /// </summary>
    public class UpdateWidgetDto
    {
        [Required]
        public string Title { get; set; }
        
        public WidgetSettingsDto Settings { get; set; }
        
        public int PositionX { get; set; }
        
        public int PositionY { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
    }
}
