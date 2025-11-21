using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates
{
    /// <summary>
    /// Aggregate root for notification configurations, managing subscription rules.
    /// </summary>
    public class NotificationConfiguration : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public UserID UserId { get; private set; }
        
        private readonly List<SubscriptionRule> _subscriptionRules;
        public IReadOnlyCollection<SubscriptionRule> SubscriptionRules => _subscriptionRules.AsReadOnly();
        
        protected NotificationConfiguration()
        {
            _subscriptionRules = new List<SubscriptionRule>();
        }
        
        /// <summary>
        /// Factory method to create a new NotificationConfiguration.
        /// </summary>
        public static NotificationConfiguration Create(
            Guid id,
            UserID userId,
            Guid? tenantId)
        {
            return new NotificationConfiguration(id, userId, tenantId);
        }
        
        private NotificationConfiguration(
            Guid id,
            UserID userId,
            Guid? tenantId) : base(id)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            TenantId = tenantId;
            _subscriptionRules = new List<SubscriptionRule>();
        }
        
        /// <summary>
        /// Adds a new subscription rule.
        /// </summary>
        public SubscriptionRule AddSubscriptionRule(
            Guid ruleId,
            NotificationEventTrigger eventTrigger,
            NotificationChannel channel)
        {
            // Check if a similar subscription already exists
            if (_subscriptionRules.Any(r => r.EventTrigger.Equals(eventTrigger) && r.Channel == channel))
            {
                throw new CollaborationDomainException("Collaboration:ErrorMessage:DuplicateSubscription");
            }
            
            var rule = new SubscriptionRule(ruleId, eventTrigger, channel);
            _subscriptionRules.Add(rule);
            
            return rule;
        }
        
        /// <summary>
        /// Updates an existing subscription rule.
        /// </summary>
        public void UpdateSubscriptionRule(
            Guid ruleId,
            NotificationChannel channel)
        {
            var rule = _subscriptionRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
            {
                throw new EntityNotFoundException(nameof(SubscriptionRule), ruleId);
            }
            
            rule.UpdateChannel(channel);
        }
        
        /// <summary>
        /// Enables a subscription rule.
        /// </summary>
        public void EnableSubscriptionRule(Guid ruleId)
        {
            var rule = _subscriptionRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
            {
                throw new EntityNotFoundException(nameof(SubscriptionRule), ruleId);
            }
            
            rule.Enable();
        }
        
        /// <summary>
        /// Disables a subscription rule.
        /// </summary>
        public void DisableSubscriptionRule(Guid ruleId)
        {
            var rule = _subscriptionRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
            {
                throw new EntityNotFoundException(nameof(SubscriptionRule), ruleId);
            }
            
            rule.Disable();
        }
        
        /// <summary>
        /// Removes a subscription rule.
        /// </summary>
        public void RemoveSubscriptionRule(Guid ruleId)
        {
            var rule = _subscriptionRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
            {
                return; // Rule already removed or doesn't exist
            }
            
            _subscriptionRules.Remove(rule);
        }
        
        /// <summary>
        /// Gets the channels to notify for a specific event.
        /// </summary>
        public List<NotificationChannel> GetNotificationChannelsForEvent(
            NotificationEventType eventType,
            TargetResourceType resourceType,
            Guid resourceId)
        {
            return _subscriptionRules
                .Where(r => r.Matches(eventType, resourceType, resourceId))
                .Select(r => r.Channel)
                .Distinct()
                .ToList();
        }
    }
}
