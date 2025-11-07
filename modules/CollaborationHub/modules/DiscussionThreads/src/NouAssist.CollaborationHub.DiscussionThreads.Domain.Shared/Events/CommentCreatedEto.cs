using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CollaborationHub.DiscussionThreads.Events
{
    [EventName("NouAssist.CollaborationHub.DiscussionThreads.CommentCreated")]
    public class CommentCreatedEto : IMultiTenant
    {
        public Guid CommentId { get; set; }
        public Guid AuthorId { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsReply { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
