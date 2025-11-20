namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents an event that can trigger notifications.
    /// </summary>
    public class NotificationEventTrigger : ValueObject
    {
        public NotificationEventType EventType { get; private set; }
        public TargetResource TargetResource { get; private set; }

        private NotificationEventTrigger() { }

        public NotificationEventTrigger(NotificationEventType eventType, TargetResource targetResource)
        {
            EventType = eventType;
            TargetResource = targetResource ?? throw new ArgumentNullException(nameof(targetResource));
        }

        /// <summary>
        /// Checks if this trigger matches the specified event.
        /// </summary>
        public bool Matches(NotificationEventType eventType, TargetResourceType resourceType, Guid resourceId)
        {
            return EventType == eventType && TargetResource.AppliesTo(resourceType, resourceId);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EventType;
            yield return TargetResource;
        }
    }
}
