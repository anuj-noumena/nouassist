using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Analytics.DTOs.Dashboard
{
    /// <summary>
    /// DTO for dashboard template information
    /// </summary>
    public class DashboardTemplateDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public Guid DashboardId { get; set; }
        
        public string DashboardName { get; set; }
    }
}
