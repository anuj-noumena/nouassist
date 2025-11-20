using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for dashboard information
    /// </summary>
    public class DashboardDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool IsDefault { get; set; }
        
        public DashboardLayoutSettingDto LayoutSetting { get; set; }
        
        public List<WidgetInstanceDto> Widgets { get; set; }
    }
}
