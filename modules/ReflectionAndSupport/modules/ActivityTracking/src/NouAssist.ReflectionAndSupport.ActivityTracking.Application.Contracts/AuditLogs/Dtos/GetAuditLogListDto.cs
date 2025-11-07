using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.AuditLogs.Dtos
{
    public class GetAuditLogListDto : PagedAndSortedResultRequestDto
    {
        public Guid? UserId { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
