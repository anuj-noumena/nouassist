using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Notification.Notifications.Notifications.Dtos
{
    public class NotificationDto : CreationAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string NotificationType { get; set; }
        public string Priority { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string ActionUrl { get; set; }
        public string ActionLabel { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public Guid? ActorId { get; set; }
        public string ActorName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsDismissed { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<NotificationDeliveryLogDto> DeliveryLogs { get; set; }
    }

    public class NotificationDeliveryLogDto
    {
        public string Channel { get; set; }
        public string Status { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string ErrorMessage { get; set; }
    }
}
