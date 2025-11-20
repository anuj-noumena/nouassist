using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Notification.Notifications.Events
{
    [EventName("NouAssist.Notifications.Delivered")]
    public class NotificationDeliveredEto : IMultiTenant
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Channel { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}
