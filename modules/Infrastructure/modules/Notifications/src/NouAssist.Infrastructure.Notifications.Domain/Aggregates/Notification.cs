using System;
using System.Collections.Generic;
using NouAssist.Infrastructure.Notifications.Entities;
using NouAssist.Infrastructure.Notifications.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Notifications.Aggregates
{
    public class Notification : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxTitleLength = 200;
        public const int MaxMessageLength = 1000;

        public Guid UserId { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public NotificationPriority Priority { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string EntityType { get; private set; }
        public Guid? EntityId { get; private set; }
        public string ActionUrl { get; private set; }
        public string ActionLabel { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Guid? ActorId { get; private set; }
        public string ActorName { get; private set; }
        public string ActorAvatar { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime? ReadAt { get; private set; }
        public bool IsDismissed { get; private set; }
        public DateTime? DismissedAt { get; private set; }
        public NotificationDeliveryStatus DeliveryStatus { get; private set; }
        public DateTime? SentAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<NotificationDeliveryLogEntry> _deliveryLogs = new();
        public IReadOnlyCollection<NotificationDeliveryLogEntry> DeliveryLogs => _deliveryLogs.AsReadOnly();

        protected Notification() { /* ORM */ }

        public Notification(
            Guid id,
            Guid userId,
            NotificationType notificationType,
            string title,
            Guid? tenantId,
            string message = null,
            NotificationPriority priority = NotificationPriority.Normal,
            int expiresInDays = 30) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            NotificationType = notificationType;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MaxTitleLength);
            TenantId = tenantId;
            Message = message;
            Priority = priority;
            
            DeliveryStatus = NotificationDeliveryStatus.Pending;
            IsRead = false;
            IsDismissed = false;
            ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays);
            
            Metadata = new Dictionary<string, object>();
        }

        public void SetMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Check.Length(message, nameof(message), MaxMessageLength);
                Message = message;
            }
        }

        public void SetEntity(string entityType, Guid entityId)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNull(entityId, nameof(entityId));
        }

        public void SetAction(string actionUrl, string actionLabel = null)
        {
            ActionUrl = Check.NotNullOrWhiteSpace(actionUrl, nameof(actionUrl));
            ActionLabel = actionLabel;
        }

        public void SetActor(Guid actorId, string actorName = null, string actorAvatar = null)
        {
            ActorId = actorId;
            ActorName = actorName;
            ActorAvatar = actorAvatar;
        }

        public void AddMetadata(string key, object value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            Metadata[key] = value;
        }

        public void MarkAsRead()
        {
            if (IsRead) return;
            
            IsRead = true;
            ReadAt = DateTime.UtcNow;
            
            if (DeliveryStatus == NotificationDeliveryStatus.Delivered)
            {
                DeliveryStatus = NotificationDeliveryStatus.Read;
            }
        }

        public void MarkAsUnread()
        {
            IsRead = false;
            ReadAt = null;
        }

        public void Dismiss()
        {
            IsDismissed = true;
            DismissedAt = DateTime.UtcNow;
        }

        public void MarkAsDelivered()
        {
            DeliveryStatus = NotificationDeliveryStatus.Delivered;
            SentAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            DeliveryStatus = NotificationDeliveryStatus.Failed;
        }

        public void AddDeliveryLog(NotificationChannel channel, NotificationDeliveryStatus status, string errorMessage = null)
        {
            var log = new NotificationDeliveryLogEntry(Id, channel, status, errorMessage);
            _deliveryLogs.Add(log);
        }

        public bool IsUnread => !IsRead;
        public bool HasAction => !string.IsNullOrWhiteSpace(ActionUrl);
        public bool HasActor => ActorId.HasValue;
        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public bool IsPending => DeliveryStatus == NotificationDeliveryStatus.Pending;
        public bool IsDelivered => DeliveryStatus == NotificationDeliveryStatus.Delivered;
        public bool IsFailed => DeliveryStatus == NotificationDeliveryStatus.Failed;
    }
}
