using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles
{
    /// <summary>
    /// Application service for user productivity profiles
    /// </summary>
    public interface IUserProductivityProfileAppService : IApplicationService
    {
        /// <summary>
        /// Gets a user's productivity profile
        /// </summary>
        Task<UserProductivityProfileDto> GetAsync(GetUserProfileDto input);
        
        /// <summary>
        /// Updates a user's cognitive load
        /// </summary>
        Task<string> UpdateCognitiveLoadAsync(UpdateCognitiveLoadDto input);
        
        /// <summary>
        /// Records a task completion
        /// </summary>
        Task RecordTaskCompletionAsync(RecordTaskCompletionDto input);
        
        /// <summary>
        /// Updates user context
        /// </summary>
        Task UpdateUserContextAsync(UpdateUserContextDto input);
        
        /// <summary>
        /// Sets a user preference
        /// </summary>
        Task SetPreferenceAsync(SetUserPreferenceDto input);
        
        /// <summary>
        /// Analyzes and updates work pattern
        /// </summary>
        Task<bool> AnalyzeWorkPatternAsync(Guid userId);
    }
}
