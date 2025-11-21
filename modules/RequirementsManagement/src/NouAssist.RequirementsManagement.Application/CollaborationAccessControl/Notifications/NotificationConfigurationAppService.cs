using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Integration;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Permissions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications
{
    [Authorize(CollaborationPermissions.Notifications.Default)]
    public class NotificationConfigurationAppService : ApplicationService, INotificationConfigurationAppService
    {
        private readonly INotificationConfigurationRepository _notificationConfigRepository;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly IResourceDisplayNameProvider _resourceDisplayNameProvider;

        public NotificationConfigurationAppService(
            INotificationConfigurationRepository notificationConfigRepository,
            IIdentityUserLookupService userLookupService,
            IResourceDisplayNameProvider resourceDisplayNameProvider)
        {
            _notificationConfigRepository = notificationConfigRepository;
            _userLookupService = userLookupService;
            _resourceDisplayNameProvider = resourceDisplayNameProvider;
        }

        public virtual async Task<NotificationConfigurationDto> GetAsync(Guid id)
        {
            try
            {
                var config = await _notificationConfigRepository.GetAsync(id);
                return await EnrichNotificationConfigurationDtoAsync(config);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindNotificationConfiguration"]);
            }
        }

        public virtual async Task<NotificationConfigurationDto> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var config = await _notificationConfigRepository.GetByUserIdAsync(userId);
                if (config == null)
                {
                    // Create a new configuration for the user if not exists
                    config = NotificationConfiguration.Create(
                        GuidGenerator.Create(),
                        new UserID(userId),
                        CurrentTenant.Id
                    );
                    
                    await _notificationConfigRepository.InsertAsync(config);
                }
                
                return await EnrichNotificationConfigurationDtoAsync(config);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindNotificationConfiguration"]);
            }
        }

        [Authorize]
        public virtual async Task<NotificationConfigurationDto> GetMyNotificationConfigurationAsync()
        {
            var currentUserId = CurrentUser.GetId();
            return await GetByUserIdAsync(currentUserId);
        }

        [Authorize(CollaborationPermissions.Notifications.SubscribeToEvents)]
        public virtual async Task<SubscriptionRuleDto> AddSubscriptionRuleAsync(AddSubscriptionRuleDto input)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var config = await _notificationConfigRepository.GetByUserIdAsync(currentUserId);
                
                if (config == null)
                {
                    config = NotificationConfiguration.Create(
                        GuidGenerator.Create(),
                        new UserID(currentUserId),
                        CurrentTenant.Id
                    );
                }
                
                var targetResource = input.ResourceId.HasValue
                    ? new TargetResource(input.ResourceType, input.ResourceId.Value)
                    : new TargetResource(input.ResourceType);
                
                var eventTrigger = new NotificationEventTrigger(input.EventType, targetResource);
                
                var rule = config.AddSubscriptionRule(
                    GuidGenerator.Create(),
                    eventTrigger,
                    input.Channel
                );
                
                await _notificationConfigRepository.UpdateAsync(config);
                
                var ruleDto = ObjectMapper.Map<SubscriptionRule, SubscriptionRuleDto>(rule);
                if (input.ResourceId.HasValue)
                {
                    ruleDto.ResourceDisplayName = await _resourceDisplayNameProvider
                        .GetResourceDisplayNameAsync(input.ResourceType, input.ResourceId);
                }
                
                return ruleDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is CollaborationDomainException domainException && 
                    domainException.Code == "Collaboration:ErrorMessage:DuplicateSubscription")
                {
                    throw new UserFriendlyException(L["Error:DuplicateSubscription"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotAddSubscription"]);
            }
        }

        [Authorize(CollaborationPermissions.Notifications.SubscribeToEvents)]
        public virtual async Task<SubscriptionRuleDto> UpdateSubscriptionRuleAsync(Guid ruleId, UpdateSubscriptionRuleDto input)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var config = await _notificationConfigRepository.GetByUserIdAsync(currentUserId);
                
                // Verify ownership of the rule
                var rule = config?.SubscriptionRules.FirstOrDefault(r => r.Id == ruleId);
                if (rule == null)
                {
                    throw new EntityNotFoundException(typeof(SubscriptionRule), ruleId);
                }
                
                config.UpdateSubscriptionRule(ruleId, input.Channel);
                
                await _notificationConfigRepository.UpdateAsync(config);
                
                // Get the updated rule
                rule = config.SubscriptionRules.First(r => r.Id == ruleId);
                var ruleDto = ObjectMapper.Map<SubscriptionRule, SubscriptionRuleDto>(rule);
                
                // Enrich with resource display name
                if (ruleDto.ResourceId.HasValue)
                {
                    ruleDto.ResourceDisplayName = await _resourceDisplayNameProvider
                        .GetResourceDisplayNameAsync(ruleDto.ResourceType, ruleDto.ResourceId);
                }
                
                return ruleDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotUpdateSubscription"]);
            }
        }

        [Authorize(CollaborationPermissions.Notifications.SubscribeToEvents)]
        public virtual async Task EnableSubscriptionRuleAsync(Guid ruleId)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var config = await _notificationConfigRepository.GetByUserIdAsync(currentUserId);
                
                // Verify ownership of the rule
                var rule = config?.SubscriptionRules.FirstOrDefault(r => r.Id == ruleId);
                if (rule == null)
                {
                    throw new EntityNotFoundException(typeof(SubscriptionRule), ruleId);
                }
                
                config.EnableSubscriptionRule(ruleId);
                await _notificationConfigRepository.UpdateAsync(config);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotEnableSubscription"]);
            }
        }

        [Authorize(CollaborationPermissions.Notifications.SubscribeToEvents)]
        public virtual async Task DisableSubscriptionRuleAsync(Guid ruleId)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var config = await _notificationConfigRepository.GetByUserIdAsync(currentUserId);
                
                // Verify ownership of the rule
                var rule = config?.SubscriptionRules.FirstOrDefault(r => r.Id == ruleId);
                if (rule == null)
                {
                    throw new EntityNotFoundException(typeof(SubscriptionRule), ruleId);
                }
                
                config.DisableSubscriptionRule(ruleId);
                await _notificationConfigRepository.UpdateAsync(config);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotDisableSubscription"]);
            }
        }

        [Authorize(CollaborationPermissions.Notifications.SubscribeToEvents)]
        public virtual async Task RemoveSubscriptionRuleAsync(Guid ruleId)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var config = await _notificationConfigRepository.GetByUserIdAsync(currentUserId);
                
                // Verify ownership of the rule
                var rule = config?.SubscriptionRules.FirstOrDefault(r => r.Id == ruleId);
                if (rule == null)
                {
                    // Rule already doesn't exist, so return success
                    return;
                }
                
                config.RemoveSubscriptionRule(ruleId);
                await _notificationConfigRepository.UpdateAsync(config);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotRemoveSubscription"]);
            }
        }

        private async Task<NotificationConfigurationDto> EnrichNotificationConfigurationDtoAsync(NotificationConfiguration config)
        {
            var dto = ObjectMapper.Map<NotificationConfiguration, NotificationConfigurationDto>(config);
            
            // Enrich with user name
            var user = await _userLookupService.FindByIdAsync(config.UserId.Value);
            if (user != null)
            {
                dto.UserName = user.UserName;
            }
            
            // Enrich subscription rules with resource display names
            foreach (var rule in dto.SubscriptionRules)
            {
                if (rule.ResourceId.HasValue)
                {
                    rule.ResourceDisplayName = await _resourceDisplayNameProvider
                        .GetResourceDisplayNameAsync(rule.ResourceType, rule.ResourceId);
                }
                else
                {
                    rule.ResourceDisplayName = L[$"ResourceType:{rule.ResourceType}"];
                }
            }
            
            return dto;
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in NotificationConfigurationAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}
