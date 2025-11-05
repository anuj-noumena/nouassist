using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Notifications.Events
{
    [EventName("NouAssist.Notifications.Sent")]
    public class NotificationSentEto : IMultiTenant
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string NotificationType { get; set; }
        public string Priority { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime SentAt { get; set; }
    }
}
