using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for dashboard view model
    /// </summary>
    public class DashboardDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the dashboard
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the dashboard
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Icon for the dashboard
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// Whether this dashboard is private to the creator
        /// </summary>
        public bool IsPrivate { get; set; }
        
        /// <summary>
        /// ID of the template this dashboard was created from, if any
        /// </summary>
        public Guid? TemplateId { get; set; }
        
        /// <summary>
        /// Tags for organizing dashboards
        /// </summary>
        public string[] Tags { get; set; }
        
        /// <summary>
        /// Layout settings
        /// </summary>
        public DashboardLayoutSettingDto LayoutSetting { get; set; }
        
        /// <summary>
        /// Refresh interval settings
        /// </summary>
        public RefreshIntervalDto RefreshInterval { get; set; }
        
        /// <summary>
        /// List of widgets on the dashboard
        /// </summary>
        public WidgetInstanceDto[] Widgets { get; set; }
    }
}
