using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Notifications.Aggregates;
using NouAssist.Notifications.Data;
using NouAssist.Notifications.Enums;
using NouAssist.Notifications.Permissions;
using NouAssist.Notifications.Preferences.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Notifications.Preferences
{
    [Authorize(NotificationsPermissions.Preferences.Default)]
    public class NotificationPreferenceAppService : ApplicationService, INotificationPreferenceAppService
    {
        private readonly INotificationPreferenceRepository _preferenceRepository;

        public NotificationPreferenceAppService(INotificationPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        public async Task<List<NotificationPreferenceDto>> GetMyPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetUserPreferencesAsync(CurrentUser.Id.Value);
            return ObjectMapper.Map<List<NotificationPreference>, List<NotificationPreferenceDto>>(preferences);
        }

        public async Task<NotificationPreferenceDto> GetPreferenceAsync(NotificationType notificationType)
        {
            var preference = await _preferenceRepository.FindByUserAndTypeAsync(
                CurrentUser.Id.Value,
                notificationType
            );

            if (preference == null)
            {
                // Return default preferences
                return new NotificationPreferenceDto
                {
                    UserId = CurrentUser.Id.Value,
                    NotificationType = notificationType.ToString(),
                    IsEnabled = true,
                    EnabledChannels = new List<string> { "Push" },
                    AllowPushNotifications = true
                };
            }

            return ObjectMapper.Map<NotificationPreference, NotificationPreferenceDto>(preference);
        }

        [Authorize(NotificationsPermissions.Preferences.Manage)]
        public async Task<NotificationPreferenceDto> UpdatePreferenceAsync(
            NotificationType notificationType,
            UpdateNotificationPreferenceDto input)
        {
            var preference = await _preferenceRepository.FindByUserAndTypeAsync(
                CurrentUser.Id.Value,
                notificationType
            );

            if (preference == null)
            {
                // Create new preference
                preference = new NotificationPreference(
                    GuidGenerator.Create(),
                    CurrentUser.Id.Value,
                    notificationType,
                    CurrentTenant.Id
                );

                await _preferenceRepository.InsertAsync(preference);
            }

            // Update settings
            if (input.IsEnabled)
            {
                preference.Enable();
            }
            else
            {
                preference.Disable();
            }

            if (input.EnabledChannels != null && input.EnabledChannels.Any())
            {
                preference.SetEnabledChannels(input.EnabledChannels);
            }

            preference.SetAlertPreferences(
                input.AllowSoundAlerts,
                input.AllowPushNotifications,
                input.AllowEmailNotifications,
                input.AllowSmsNotifications
            );

            if (!string.IsNullOrWhiteSpace(input.QuietStartTime) && !string.IsNullOrWhiteSpace(input.QuietEndTime))
            {
                preference.SetQuietHours(input.QuietStartTime, input.QuietEndTime);
            }

            await _preferenceRepository.UpdateAsync(preference);

            return ObjectMapper.Map<NotificationPreference, NotificationPreferenceDto>(preference);
        }

        [Authorize(NotificationsPermissions.Preferences.Manage)]
        public async Task ResetToDefaultsAsync()
        {
            var preferences = await _preferenceRepository.GetUserPreferencesAsync(CurrentUser.Id.Value);

            foreach (var preference in preferences)
            {
                await _preferenceRepository.DeleteAsync(preference);
            }
        }
    }
}
