using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Interface for notification dispatch service.
    /// </summary>
    public interface INotificationDispatchService
    {
        Task<Dictionary<Guid, List<NotificationChannel>>> GetNotificationPreferencesAsync(
            NotificationEventType eventType,
            TargetResourceType resourceType,
            Guid resourceId,
            List<Guid> affectedUserIds);
            
        Task<NotificationChannel> GetDefaultChannelForUserAsync(Guid userId);
    }
}
