using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Data;
using NouAssist.IdentityAccess.Users.Permissions;
using NouAssist.IdentityAccess.Users.Users;
using NouAssist.IdentityAccess.Users.Users.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Users
{
    [Authorize(UsersPermissions.Users.ManageSettings)]
    public class UserSettingsAppService : ApplicationService, IUserSettingsAppService
    {
        private readonly IUserSettingsRepository _settingsRepository;

        public UserSettingsAppService(IUserSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<UserSettingsDto> GetMySettingsAsync()
        {
            var userId = CurrentUser.GetId();
            var settings = await _settingsRepository.FindByUserIdAsync(userId);

            if (settings == null)
            {
                settings = new UserSettings(GuidGenerator.Create(), userId);
                await _settingsRepository.InsertAsync(settings);
            }

            return ObjectMapper.Map<UserSettings, UserSettingsDto>(settings);
        }

        public async Task<UserSettingsDto> UpdateSettingsAsync(UpdateUserSettingsDto input)
        {
            var userId = CurrentUser.GetId();
            var settings = await _settingsRepository.FindByUserIdAsync(userId);

            if (settings == null)
            {
                throw new UserFriendlyException("User settings not found.");
            }

            if (!string.IsNullOrWhiteSpace(input.Theme))
            {
                settings.UpdateTheme(input.Theme);
            }

            if (input.EnableNotifications.HasValue)
            {
                settings.EnableNotifications = input.EnableNotifications.Value;
            }

            if (input.EnableSounds.HasValue)
            {
                settings.EnableSounds = input.EnableSounds.Value;
            }

            if (input.EnableDarkMode.HasValue)
            {
                settings.EnableDarkMode = input.EnableDarkMode.Value;
            }

            if (!string.IsNullOrWhiteSpace(input.Language))
            {
                settings.Language = input.Language;
            }

            if (!string.IsNullOrWhiteSpace(input.Timezone))
            {
                settings.Timezone = input.Timezone;
            }

            if (!string.IsNullOrWhiteSpace(input.DateFormat))
            {
                settings.DateFormat = input.DateFormat;
            }

            if (!string.IsNullOrWhiteSpace(input.TimeFormat))
            {
                settings.TimeFormat = input.TimeFormat;
            }

            if (!string.IsNullOrWhiteSpace(input.WeekStartDay))
            {
                settings.WeekStartDay = input.WeekStartDay;
            }

            await _settingsRepository.UpdateAsync(settings);
            return ObjectMapper.Map<UserSettings, UserSettingsDto>(settings);
        }

        public async Task<UserSettingsDto> UpdatePomodoroSettingsAsync(UpdatePomodoroSettingsDto input)
        {
            var userId = CurrentUser.GetId();
            var settings = await _settingsRepository.FindByUserIdAsync(userId);

            if (settings == null)
            {
                throw new UserFriendlyException("User settings not found.");
            }

            settings.UpdatePomodoroSettings(
                input.DefaultPomodoroMinutes,
                input.DefaultShortBreakMinutes,
                input.DefaultLongBreakMinutes
            );

            await _settingsRepository.UpdateAsync(settings);
            return ObjectMapper.Map<UserSettings, UserSettingsDto>(settings);
        }

        public async Task<UserSettingsDto> ToggleDarkModeAsync()
        {
            var userId = CurrentUser.GetId();
            var settings = await _settingsRepository.FindByUserIdAsync(userId);

            if (settings == null)
            {
                throw new UserFriendlyException("User settings not found.");
            }

            settings.ToggleDarkMode();
            await _settingsRepository.UpdateAsync(settings);
            return ObjectMapper.Map<UserSettings, UserSettingsDto>(settings);
        }
    }
}
