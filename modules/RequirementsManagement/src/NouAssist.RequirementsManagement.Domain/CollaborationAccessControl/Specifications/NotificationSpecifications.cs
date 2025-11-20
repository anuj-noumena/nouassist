namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Specifications
{
    /// <summary>
    /// Base specification for subscription rules.
    /// </summary>
    public abstract class SubscriptionRuleSpecification : Specification<SubscriptionRule>
    {
    }

    /// <summary>
    /// Specification for active subscription rules.
    /// </summary>
    public class ActiveSubscriptionRuleSpecification : SubscriptionRuleSpecification
    {
        public override Expression<Func<SubscriptionRule, bool>> ToExpression()
        {
            return rule => rule.IsEnabled;
        }
    }

    /// <summary>
    /// Specification for subscription rules matching a specific event.
    /// </summary>
    public class MatchingEventSubscriptionRuleSpecification : SubscriptionRuleSpecification
    {
        private readonly NotificationEventType _eventType;
        private readonly TargetResourceType _resourceType;
        private readonly Guid _resourceId;

        public MatchingEventSubscriptionRuleSpecification(
            NotificationEventType eventType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            _eventType = eventType;
            _resourceType = resourceType;
            _resourceId = resourceId;
        }

        public override Expression<Func<SubscriptionRule, bool>> ToExpression()
        {
            return rule => rule.Matches(_eventType, _resourceType, _resourceId);
        }
    }

    /// <summary>
    /// Specification for subscription rules using a specific notification channel.
    /// </summary>
    public class ChannelSubscriptionRuleSpecification : SubscriptionRuleSpecification
    {
        private readonly NotificationChannel _channel;

        public ChannelSubscriptionRuleSpecification(NotificationChannel channel)
        {
            _channel = channel;
        }

        public override Expression<Func<SubscriptionRule, bool>> ToExpression()
        {
            return rule => rule.Channel == _channel;
        }
    }
}
