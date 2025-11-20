namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos
{
    public class NotificationConfigurationDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } // Display name
        public List<SubscriptionRuleDto> SubscriptionRules { get; set; }
    }
}
