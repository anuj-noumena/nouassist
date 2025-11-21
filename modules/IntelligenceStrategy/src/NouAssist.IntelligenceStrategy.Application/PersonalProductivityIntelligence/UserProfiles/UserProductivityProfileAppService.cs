using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Localization;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Permissions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles
{
    /// <summary>
    /// Implementation of the user productivity profile application service
    /// </summary>
    [Authorize]
    public class UserProductivityProfileAppService : ApplicationService, IUserProductivityProfileAppService
    {
        private readonly IUserProductivityProfileRepository _userProfileRepository;
        private readonly IUserProductivityProfileManager _userProfileManager;
        private readonly ILogger<UserProductivityProfileAppService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserProductivityProfileAppService(
            IUserProductivityProfileRepository userProfileRepository,
            IUserProductivityProfileManager userProfileManager,
            ILogger<UserProductivityProfileAppService> logger)
        {
            _userProfileRepository = userProfileRepository;
            _userProfileManager = userProfileManager;
            _logger = logger;
            
            LocalizationResource = typeof(PersonalProductivityIntelligenceResource);
        }

        /// <summary>
        /// Gets a user's productivity profile
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.View)]
        public async Task<UserProductivityProfileDto> GetAsync(GetUserProfileDto input)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(input.UserId);
                
                if (profile == null)
                {
                    // Create profile if it doesn't exist
                    profile = await _userProfileManager.GetOrCreateProfileAsync(input.UserId, CurrentTenant.Id);
                }
                
                // Map to DTO with appropriate includes
                var profileDto = ObjectMapper.Map<UserProductivityProfile, UserProductivityProfileDto>(profile);
                
                // Filter collections based on input parameters
                if (!input.IncludePerformanceData)
                {
                    profileDto.RecentPerformanceData = new List<PerformanceDataPointDto>();
                }
                else if (profileDto.RecentPerformanceData?.Count > input.MaxPerformanceDataCount)
                {
                    profileDto.RecentPerformanceData = profileDto.RecentPerformanceData
                        .OrderByDescending(p => p.Timestamp)
                        .Take(input.MaxPerformanceDataCount)
                        .ToList();
                }
                
                if (!input.IncludeTaskCompletions)
                {
                    profileDto.RecentTaskCompletions = new List<TaskCompletionRecordDto>();
                }
                else if (profileDto.RecentTaskCompletions?.Count > input.MaxTaskCompletionsCount)
                {
                    profileDto.RecentTaskCompletions = profileDto.RecentTaskCompletions
                        .OrderByDescending(t => t.CompletedAt)
                        .Take(input.MaxTaskCompletionsCount)
                        .ToList();
                }
                
                return profileDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "GetAsync");
                throw;
            }
        }

        /// <summary>
        /// Updates a user's cognitive load
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.UpdateCognitiveLoad)]
        public async Task<string> UpdateCognitiveLoadAsync(UpdateCognitiveLoadDto input)
        {
            try
            {
                var cognitiveLoadLevel = await _userProfileManager.UpdateCognitiveLoadAsync(input.UserId, input.LoadValue);
                
                return cognitiveLoadLevel.ToString();
            }
            catch (UserProfileNotFoundException ex)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "UpdateCognitiveLoadAsync");
                throw;
            }
        }

        /// <summary>
        /// Records a task completion
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.RecordTaskCompletion)]
        public async Task RecordTaskCompletionAsync(RecordTaskCompletionDto input)
        {
            try
            {
                // Map TaskContextDto to TaskContext value object
                TaskContext taskContext = null;
                if (input.TaskContext != null)
                {
                    taskContext = new TaskContext(
                        input.TaskContext.ProjectId,
                        input.TaskContext.TeamId,
                        input.TaskContext.Tags,
                        input.TaskContext.Location,
                        input.TaskContext.EnergyLevel,
                        input.TaskContext.FocusLevel);
                }
                
                await _userProfileManager.RecordTaskCompletionAsync(
                    input.UserId,
                    input.TaskId,
                    input.TaskName,
                    input.EstimatedDuration,
                    input.ActualDuration,
                    taskContext);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "RecordTaskCompletionAsync");
                throw;
            }
        }

        /// <summary>
        /// Updates user context
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.UpdateContext)]
        public async Task UpdateUserContextAsync(UpdateUserContextDto input)
        {
            try
            {
                await _userProfileManager.UpdateUserContextAsync(
                    input.UserId,
                    input.EntityType,
                    input.EntityId,
                    input.Properties);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "UpdateUserContextAsync");
                throw;
            }
        }

        /// <summary>
        /// Sets a user preference
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.SetPreference)]
        public async Task SetPreferenceAsync(SetUserPreferenceDto input)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(input.UserId);
                
                if (profile == null)
                {
                    profile = await _userProfileManager.GetOrCreateProfileAsync(input.UserId, CurrentTenant.Id);
                }
                
                profile.SetPreference(input.Key, input.Value);
                await _userProfileRepository.UpdateAsync(profile);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "SetPreferenceAsync");
                throw;
            }
        }

        /// <summary>
        /// Analyzes and updates work pattern
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.UserProfile.AnalyzeWorkPattern)]
        public async Task<bool> AnalyzeWorkPatternAsync(Guid userId)
        {
            try
            {
                return await _userProfileManager.AnalyzeAndUpdateWorkPatternAsync(userId);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "AnalyzeWorkPatternAsync");
                throw;
            }
        }
        
        private async Task HandleErrorAsync(Exception exception, string methodName)
        {
            _logger.LogError(exception, "Error in {AppServiceName}.{MethodName}: {ErrorMessage}", 
                GetType().Name, methodName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
