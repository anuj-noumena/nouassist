using System;
using System.Threading.Tasks;
using NouAssist.Users.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Users.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<UserDto> GetMyProfileAsync();
        Task<UserDto> UpdateProfileAsync(UpdateUserProfileDto input);
        Task UpdateLastLoginAsync();
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
    }

    public interface IUserSettingsAppService : IApplicationService
    {
        Task<UserSettingsDto> GetMySettingsAsync();
        Task<UserSettingsDto> UpdateSettingsAsync(UpdateUserSettingsDto input);
        Task<UserSettingsDto> UpdatePomodoroSettingsAsync(UpdatePomodoroSettingsDto input);
        Task<UserSettingsDto> ToggleDarkModeAsync();
    }

    public interface IUserEisenhowerPreferenceAppService : IApplicationService
    {
        Task<UserEisenhowerPreferenceDto> GetMyPreferenceAsync();
        Task<UserEisenhowerPreferenceDto> UpdatePreferenceAsync(UpdateEisenhowerPreferenceDto input);
        Task<UserEisenhowerPreferenceDto> EnableAutoClassificationAsync();
        Task<UserEisenhowerPreferenceDto> DisableAutoClassificationAsync();
    }
}
