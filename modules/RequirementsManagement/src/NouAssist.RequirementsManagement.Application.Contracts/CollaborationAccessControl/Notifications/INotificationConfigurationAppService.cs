namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications
{
    public interface INotificationConfigurationAppService : IApplicationService
    {
        Task<NotificationConfigurationDto> GetAsync(Guid id);
        Task<NotificationConfigurationDto> GetByUserIdAsync(Guid userId);
        Task<NotificationConfigurationDto> GetMyNotificationConfigurationAsync();
        
        Task<SubscriptionRuleDto> AddSubscriptionRuleAsync(AddSubscriptionRuleDto input);
        Task<SubscriptionRuleDto> UpdateSubscriptionRuleAsync(Guid ruleId, UpdateSubscriptionRuleDto input);
        Task EnableSubscriptionRuleAsync(Guid ruleId);
        Task DisableSubscriptionRuleAsync(Guid ruleId);
        Task RemoveSubscriptionRuleAsync(Guid ruleId);
    }
}
