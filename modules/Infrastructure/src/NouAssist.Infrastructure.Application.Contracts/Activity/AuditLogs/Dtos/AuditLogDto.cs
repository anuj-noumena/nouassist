using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Activity.AuditLogs.Dtos
{
    public class AuditLogDto : CreationAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Category { get; set; }
        public string Severity { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public Dictionary<string, object> ChangedFields { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
