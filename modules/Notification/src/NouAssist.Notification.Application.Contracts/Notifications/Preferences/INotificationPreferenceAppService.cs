using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Notifications.Enums;
using NouAssist.Notifications.Preferences.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Notifications.Preferences
{
    public interface INotificationPreferenceAppService : IApplicationService
    {
        Task<List<NotificationPreferenceDto>> GetMyPreferencesAsync();
        Task<NotificationPreferenceDto> GetPreferenceAsync(NotificationType notificationType);
        Task<NotificationPreferenceDto> UpdatePreferenceAsync(NotificationType notificationType, UpdateNotificationPreferenceDto input);
        Task ResetToDefaultsAsync();
    }
}
