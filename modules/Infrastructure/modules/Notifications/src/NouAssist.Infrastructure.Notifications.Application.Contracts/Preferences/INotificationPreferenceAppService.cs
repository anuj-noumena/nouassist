using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Notifications.Enums;
using NouAssist.Infrastructure.Notifications.Preferences.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Notifications.Preferences
{
    public interface INotificationPreferenceAppService : IApplicationService
    {
        Task<List<NotificationPreferenceDto>> GetMyPreferencesAsync();
        Task<NotificationPreferenceDto> GetPreferenceAsync(NotificationType notificationType);
        Task<NotificationPreferenceDto> UpdatePreferenceAsync(NotificationType notificationType, UpdateNotificationPreferenceDto input);
        Task ResetToDefaultsAsync();
    }
}
