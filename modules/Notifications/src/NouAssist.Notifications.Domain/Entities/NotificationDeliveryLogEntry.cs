using System;
using NouAssist.Notifications.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Notifications.Entities
{
    public class NotificationDeliveryLogEntry : Entity
    {
        public Guid NotificationId { get; private set; }
        public NotificationChannel Channel { get; private set; }
        public NotificationDeliveryStatus Status { get; private set; }
        public DateTime? AttemptedAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }
        public DateTime? FailedAt { get; private set; }
        public string ErrorMessage { get; private set; }

        protected NotificationDeliveryLogEntry() { /* ORM */ }

        public NotificationDeliveryLogEntry(
            Guid notificationId,
            NotificationChannel channel,
            NotificationDeliveryStatus status,
            string errorMessage = null)
        {
            NotificationId = notificationId;
            Channel = channel;
            Status = status;
            ErrorMessage = errorMessage;
            
            AttemptedAt = DateTime.UtcNow;
            
            if (status == NotificationDeliveryStatus.Delivered)
            {
                DeliveredAt = DateTime.UtcNow;
            }
            else if (status == NotificationDeliveryStatus.Failed)
            {
                FailedAt = DateTime.UtcNow;
            }
        }

        public void MarkAsDelivered()
        {
            Status = NotificationDeliveryStatus.Delivered;
            DeliveredAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationDeliveryStatus.Failed;
            FailedAt = DateTime.UtcNow;
            ErrorMessage = errorMessage;
        }

        public bool WasDelivered => Status == NotificationDeliveryStatus.Delivered;
        public bool HasFailed => Status == NotificationDeliveryStatus.Failed;
        public bool IsPending => Status == NotificationDeliveryStatus.Pending;

        public override object[] GetKeys() => new object[] { NotificationId, Channel, AttemptedAt };
    }
}
