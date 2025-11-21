using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Services
{
    /// <summary>
    /// Manages user productivity profiles
    /// </summary>
    public class UserProductivityProfileManager : DomainService, IUserProductivityProfileManager
    {
        private readonly IUserProductivityProfileRepository _userProfileRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<UserProductivityProfileManager> _logger;

        /// <summary>
        /// Creates a new UserProductivityProfileManager
        /// </summary>
        public UserProductivityProfileManager(
            IUserProductivityProfileRepository userProfileRepository,
            IGuidGenerator guidGenerator,
            ILogger<UserProductivityProfileManager> logger)
        {
            _userProfileRepository = userProfileRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Gets or creates a user productivity profile
        /// </summary>
        public async Task<UserProductivityProfile> GetOrCreateProfileAsync(Guid userId, Guid? tenantId = null)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (profile == null)
                {
                    profile = UserProductivityProfile.Create(
                        _guidGenerator.Create(),
                        userId,
                        tenantId);
                    
                    await _userProfileRepository.InsertAsync(profile);
                    _logger.LogInformation("Created new productivity profile for user {UserId}", userId);
                }
                
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting or creating productivity profile for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Updates cognitive load for a user
        /// </summary>
        public async Task<CognitiveLoadLevel> UpdateCognitiveLoadAsync(Guid userId, double loadValue)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (profile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                profile.UpdateCognitiveLoad(loadValue, DateTime.UtcNow);
                await _userProfileRepository.UpdateAsync(profile);
                
                return profile.CognitiveLoadLevel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cognitive load for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Analyzes and updates a user's work pattern model
        /// </summary>
        public async Task<bool> AnalyzeAndUpdateWorkPatternAsync(Guid userId)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (profile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                // In a real implementation, this would use ML or analytics to detect patterns
                // For this example, we're creating a simple pattern
                var patternDescription = "Most productive between 9-11am, tends to complete tasks 10% faster than estimated";
                var patternConfidence = 0.85;
                var modelData = "{\"productiveHours\": [9, 10, 11], \"estimationBias\": 0.9}";
                
                profile.UpdateWorkPattern(
                    patternDescription,
                    patternConfidence,
                    modelData,
                    DateTime.UtcNow);
                
                await _userProfileRepository.UpdateAsync(profile);
                
                _logger.LogInformation("Updated work pattern for user {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing work pattern for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Records a task completion
        /// </summary>
        public async Task RecordTaskCompletionAsync(
            Guid userId,
            Guid taskId,
            string taskName,
            int estimatedDuration,
            int actualDuration,
            TaskContext taskContext = null)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (profile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                profile.RecordTaskCompletion(
                    taskId,
                    taskName,
                    estimatedDuration,
                    actualDuration,
                    DateTime.UtcNow,
                    taskContext);
                
                await _userProfileRepository.UpdateAsync(profile);
                
                _logger.LogInformation("Recorded task completion for user {UserId}, task {TaskId}", userId, taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording task completion for user {UserId}, task {TaskId}", userId, taskId);
                throw;
            }
        }

        /// <summary>
        /// Updates user context references
        /// </summary>
        public async Task UpdateUserContextAsync(
            Guid userId,
            string entityType,
            Guid entityId,
            Dictionary<string, string> properties = null)
        {
            try
            {
                var profile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (profile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                profile.AddOrUpdateExternalContext(entityType, entityId, properties);
                await _userProfileRepository.UpdateAsync(profile);
                
                _logger.LogInformation(
                    "Updated user context for user {UserId}, entity type {EntityType}, entity ID {EntityId}",
                    userId, entityType, entityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating user context for user {UserId}, entity type {EntityType}",
                    userId, entityType);
                throw;
            }
        }
    }
}
