using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Activity.ActivityFeeds.Dtos
{
    public class ActivityFeedDto : FullAuditedEntityDto<Guid>
    {
        public Guid ActorId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityTitle { get; set; }
        public Guid? TargetUserId { get; set; }
        public string Description { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public Dictionary<string, object> Changes { get; set; }
        public string Visibility { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
