using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Notification.Notifications.Enums;
using NouAssist.Notification.Notifications.Preferences.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Notification.Notifications.Preferences
{
    public interface INotificationPreferenceAppService : IApplicationService
    {
        Task<List<NotificationPreferenceDto>> GetMyPreferencesAsync();
        Task<NotificationPreferenceDto> GetPreferenceAsync(NotificationType notificationType);
        Task<NotificationPreferenceDto> UpdatePreferenceAsync(NotificationType notificationType, UpdateNotificationPreferenceDto input);
        Task ResetToDefaultsAsync();
    }
}
