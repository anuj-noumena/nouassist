using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityHistory.Dtos
{
    public class EntityHistoryDto : CreationAuditedEntityDto<Guid>
    {
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public int Version { get; set; }
        public Guid ActorId { get; set; }
        public string ActorName { get; set; }
        public string Action { get; set; }
        public string ChangeDescription { get; set; }
        public Dictionary<string, object> PreviousState { get; set; }
        public Dictionary<string, object> CurrentState { get; set; }
        public Dictionary<string, object> Changes { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public string IpAddress { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
