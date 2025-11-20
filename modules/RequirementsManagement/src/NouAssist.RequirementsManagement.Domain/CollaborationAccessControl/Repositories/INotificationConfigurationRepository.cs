namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories
{
    /// <summary>
    /// Repository for NotificationConfiguration aggregate.
    /// </summary>
    public interface INotificationConfigurationRepository : IRepository<NotificationConfiguration, Guid>
    {
        /// <summary>
        /// Gets a notification configuration by user ID.
        /// </summary>
        Task<NotificationConfiguration> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all notification configurations with subscription rules matching the specified event.
        /// </summary>
        Task<List<NotificationConfiguration>> GetByMatchingEventAsync(
            NotificationEventType eventType,
            TargetResourceType resourceType,
            Guid resourceId,
            CancellationToken cancellationToken = default);
    }
}
