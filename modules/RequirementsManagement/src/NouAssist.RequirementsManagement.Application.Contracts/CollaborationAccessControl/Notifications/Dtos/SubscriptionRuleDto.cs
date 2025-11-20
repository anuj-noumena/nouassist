using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos
{
    public class SubscriptionRuleDto : EntityDto<Guid>
    {
        public NotificationEventType EventType { get; set; }
        public TargetResourceType ResourceType { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceDisplayName { get; set; }
        public NotificationChannel Channel { get; set; }
        public bool IsEnabled { get; set; }
    }
}
