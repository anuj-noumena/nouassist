namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Entities
{
    /// <summary>
    /// Represents a rule for subscribing to notifications based on specific events.
    /// </summary>
    public class SubscriptionRule : Entity<Guid>
    {
        public NotificationEventTrigger EventTrigger { get; private set; }
        public NotificationChannel Channel { get; private set; }
        public bool IsEnabled { get; private set; }
        
        protected SubscriptionRule() { }
        
        internal SubscriptionRule(
            Guid id,
            NotificationEventTrigger eventTrigger,
            NotificationChannel channel) : base(id)
        {
            EventTrigger = eventTrigger ?? throw new ArgumentNullException(nameof(eventTrigger));
            Channel = channel;
            IsEnabled = true;
        }
        
        /// <summary>
        /// Enables the subscription rule.
        /// </summary>
        internal void Enable()
        {
            IsEnabled = true;
        }
        
        /// <summary>
        /// Disables the subscription rule.
        /// </summary>
        internal void Disable()
        {
            IsEnabled = false;
        }
        
        /// <summary>
        /// Updates the notification channel.
        /// </summary>
        internal void UpdateChannel(NotificationChannel channel)
        {
            Channel = channel;
        }
        
        /// <summary>
        /// Checks if this subscription rule matches the specified event.
        /// </summary>
        public bool Matches(NotificationEventType eventType, TargetResourceType resourceType, Guid resourceId)
        {
            return IsEnabled && EventTrigger.Matches(eventType, resourceType, resourceId);
        }
    }
}
