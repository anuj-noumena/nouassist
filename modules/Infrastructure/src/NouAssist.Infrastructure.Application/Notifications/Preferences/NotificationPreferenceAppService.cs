using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Data;
using NouAssist.Infrastructure.Permissions;
using NouAssist.Notification.Notifications.Enums;
using NouAssist.Notification.Notifications.Preferences;
using NouAssist.Notification.Notifications.Preferences.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Notifications.Preferences
{
    [Authorize(InfrastructurePermissions.Notifications.Preferences.Default)]
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

        [Authorize(InfrastructurePermissions.Notifications.Preferences.Manage)]
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
                preference.Activate();
            }
            else
            {
                preference.Deactivate();
            }

            if (input.EnabledChannels != null && input.EnabledChannels.Any())
            {
                // Reset all channels first
                preference.DisableAll();

                foreach (var channel in input.EnabledChannels)
                {
                    if (Enum.TryParse<NotificationChannel>(channel, true, out var channelEnum))
                    {
                        switch (channelEnum)
                        {
                            case NotificationChannel.Push:
                                preference.EnablePush();
                                break;
                            case NotificationChannel.Email:
                                preference.EnableEmail();
                                break;
                            case NotificationChannel.Sms:
                                preference.EnableSms();
                                break;
                        }
                    }
                }
            }

            if (input.AllowPushNotifications) preference.EnablePush(); else preference.DisablePush();
            if (input.AllowEmailNotifications) preference.EnableEmail(); else preference.DisableEmail();
            if (input.AllowSmsNotifications) preference.EnableSms(); else preference.DisableSms();

            if (!string.IsNullOrWhiteSpace(input.QuietStartTime) && !string.IsNullOrWhiteSpace(input.QuietEndTime))
            {
                preference.SetCustomSchedule($"{input.QuietStartTime}-{input.QuietEndTime}");
            }

            await _preferenceRepository.UpdateAsync(preference);

            return ObjectMapper.Map<NotificationPreference, NotificationPreferenceDto>(preference);
        }

        [Authorize(InfrastructurePermissions.Notifications.Preferences.Manage)]
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
