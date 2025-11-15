using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Events
{
    [EventName("NouAssist.Learning.TopicCreated")]
    public class TopicCreatedEto : IMultiTenant
    {
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public bool IsFolder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
