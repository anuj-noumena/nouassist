using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos
{
    public class AddSubscriptionRuleDto
    {
        [Required]
        public NotificationEventType EventType { get; set; }

        [Required]
        public TargetResourceType ResourceType { get; set; }

        public Guid? ResourceId { get; set; }

        [Required]
        public NotificationChannel Channel { get; set; }
    }
}
