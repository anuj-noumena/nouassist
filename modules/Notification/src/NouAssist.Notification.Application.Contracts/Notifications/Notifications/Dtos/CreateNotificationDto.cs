using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.Notifications.Enums;

namespace NouAssist.Notifications.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string ActionUrl { get; set; }
        public string ActionLabel { get; set; }
        public Guid? ActorId { get; set; }
        public int? ExpiresInDays { get; set; }
    }
}
