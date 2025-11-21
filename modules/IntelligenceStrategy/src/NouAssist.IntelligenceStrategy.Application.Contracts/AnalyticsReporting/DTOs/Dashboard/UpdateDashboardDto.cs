using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for updating an existing dashboard
    /// </summary>
    public class UpdateDashboardDto
    {
        [Required]
        [StringLength(AnalyticsConsts.Dashboard.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(AnalyticsConsts.Dashboard.MaxDescriptionLength)]
        public string Description { get; set; }
        
        public bool IsDefault { get; set; }
        
        public DashboardLayoutSettingDto LayoutSetting { get; set; }
    }
}
