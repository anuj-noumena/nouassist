using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Notification.Notifications.Events
{
    [EventName("NouAssist.Notifications.DeliveryFailed")]
    public class NotificationDeliveryFailedEto : IMultiTenant
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Channel { get; set; }
        public string ErrorMessage { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime FailedAt { get; set; }
    }
}
