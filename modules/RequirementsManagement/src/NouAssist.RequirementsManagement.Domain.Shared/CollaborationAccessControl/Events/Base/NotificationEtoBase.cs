namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Events.Base
{
    /// <summary>
    /// Base class for notification-related event transfer objects.
    /// </summary>
    [Serializable]
    public abstract class NotificationEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public NotificationEventType EventType { get; set; }
        public TargetResourceType ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid InitiatorUserId { get; set; }
    }
}
