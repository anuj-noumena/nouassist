using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using NouAssist.CollaborationHub.UserProfiles.Data;
using NouAssist.CollaborationHub.UserProfiles.Dtos;
using NouAssist.CollaborationHub.UserProfiles.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.UserProfiles.Users
{
    [Authorize(UsersPermissions.Users.ManageSettings)]
    public class UserEisenhowerPreferenceAppService : ApplicationService, IUserEisenhowerPreferenceAppService
    {
        private readonly IUserEisenhowerPreferenceRepository _preferenceRepository;

        public UserEisenhowerPreferenceAppService(
            IUserEisenhowerPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        public async Task<UserEisenhowerPreferenceDto> GetMyPreferenceAsync()
        {
            var userId = CurrentUser.GetId();
            var preference = await _preferenceRepository.FindByUserIdAsync(userId);

            if (preference == null)
            {
                preference = new UserEisenhowerPreference(GuidGenerator.Create(), userId);
                await _preferenceRepository.InsertAsync(preference);
            }

            return ObjectMapper.Map<UserEisenhowerPreference, UserEisenhowerPreferenceDto>(preference);
        }

        public async Task<UserEisenhowerPreferenceDto> UpdatePreferenceAsync(UpdateEisenhowerPreferenceDto input)
        {
            var userId = CurrentUser.GetId();
            var preference = await _preferenceRepository.FindByUserIdAsync(userId);

            if (preference == null)
            {
                throw new UserFriendlyException("User preference not found.");
            }

            preference.UpdateThresholds(
                input.ImportanceThreshold,
                input.UrgencyThreshold
            );

            if (input.UseAutoClassification.HasValue)
            {
                if (input.UseAutoClassification.Value)
                    preference.EnableAutoClassification();
                else
                    preference.DisableAutoClassification();
            }

            await _preferenceRepository.UpdateAsync(preference);
            return ObjectMapper.Map<UserEisenhowerPreference, UserEisenhowerPreferenceDto>(preference);
        }

        public async Task<UserEisenhowerPreferenceDto> EnableAutoClassificationAsync()
        {
            var userId = CurrentUser.GetId();
            var preference = await _preferenceRepository.FindByUserIdAsync(userId);

            if (preference == null)
            {
                throw new UserFriendlyException("User preference not found.");
            }

            preference.EnableAutoClassification();
            await _preferenceRepository.UpdateAsync(preference);
            return ObjectMapper.Map<UserEisenhowerPreference, UserEisenhowerPreferenceDto>(preference);
        }

        public async Task<UserEisenhowerPreferenceDto> DisableAutoClassificationAsync()
        {
            var userId = CurrentUser.GetId();
            var preference = await _preferenceRepository.FindByUserIdAsync(userId);

            if (preference == null)
            {
                throw new UserFriendlyException("User preference not found.");
            }

            preference.DisableAutoClassification();
            await _preferenceRepository.UpdateAsync(preference);
            return ObjectMapper.Map<UserEisenhowerPreference, UserEisenhowerPreferenceDto>(preference);
        }
    }
}
