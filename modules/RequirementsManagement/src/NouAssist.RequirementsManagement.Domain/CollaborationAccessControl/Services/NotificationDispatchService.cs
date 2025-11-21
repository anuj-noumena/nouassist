using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Domain service for dispatching notifications based on configured rules.
    /// </summary>
    public class NotificationDispatchService : DomainService, INotificationDispatchService
    {
        private readonly INotificationConfigurationRepository _notificationConfigRepository;
        private readonly ILogger<NotificationDispatchService> _logger;

        public NotificationDispatchService(
            INotificationConfigurationRepository notificationConfigRepository,
            ILogger<NotificationDispatchService> logger)
        {
            _notificationConfigRepository = notificationConfigRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets notification preferences for users affected by an event.
        /// </summary>
        public async Task<Dictionary<Guid, List<NotificationChannel>>> GetNotificationPreferencesAsync(
            NotificationEventType eventType,
            TargetResourceType resourceType,
            Guid resourceId,
            List<Guid> affectedUserIds)
        {
            var result = new Dictionary<Guid, List<NotificationChannel>>();
            
            try
            {
                // Get all users who have subscribed to this event
                var configurations = await _notificationConfigRepository.GetByMatchingEventAsync(
                    eventType, resourceType, resourceId);
                
                foreach (var config in configurations)
                {
                    // Only include affected users
                    if (affectedUserIds.Contains(config.UserId.Value))
                    {
                        var channels = config.GetNotificationChannelsForEvent(
                            eventType, resourceType, resourceId);
                            
                        if (channels.Any())
                        {
                            result[config.UserId.Value] = channels;
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error getting notification preferences for event {EventType} on {ResourceType} with ID {ResourceId}",
                    eventType, resourceType, resourceId);
                return result;
            }
        }

        /// <summary>
        /// Gets the default notification channel for a user.
        /// </summary>
        public async Task<NotificationChannel> GetDefaultChannelForUserAsync(Guid userId)
        {
            try
            {
                var config = await _notificationConfigRepository.GetByUserIdAsync(userId);
                
                if (config == null)
                {
                    return NotificationChannel.InApp; // Default channel if no configuration exists
                }
                
                // Look for a rule with InApp channel first, as it's the most common
                var inAppRule = config.SubscriptionRules
                    .FirstOrDefault(r => r.IsEnabled && r.Channel == NotificationChannel.InApp);
                    
                if (inAppRule != null)
                {
                    return NotificationChannel.InApp;
                }
                
                // Otherwise, use the first enabled rule's channel
                var firstEnabledRule = config.SubscriptionRules
                    .FirstOrDefault(r => r.IsEnabled);
                    
                return firstEnabledRule?.Channel ?? NotificationChannel.InApp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default notification channel for user {UserId}", userId);
                return NotificationChannel.InApp;
            }
        }
    }
}
