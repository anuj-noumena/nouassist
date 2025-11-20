using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services
{
    /// <summary>
    /// Defines operations for managing user productivity profiles
    /// </summary>
    public interface IUserProductivityProfileManager
    {
        /// <summary>
        /// Gets or creates a user productivity profile
        /// </summary>
        Task<UserProductivityProfile> GetOrCreateProfileAsync(Guid userId, Guid? tenantId = null);
        
        /// <summary>
        /// Updates cognitive load for a user
        /// </summary>
        Task<CognitiveLoadLevel> UpdateCognitiveLoadAsync(Guid userId, double loadValue);
        
        /// <summary>
        /// Analyzes and updates a user's work pattern model
        /// </summary>
        Task<bool> AnalyzeAndUpdateWorkPatternAsync(Guid userId);
        
        /// <summary>
        /// Records a task completion
        /// </summary>
        Task RecordTaskCompletionAsync(
            Guid userId,
            Guid taskId,
            string taskName,
            int estimatedDuration,
            int actualDuration,
            TaskContext taskContext = null);
        
        /// <summary>
        /// Updates user context references
        /// </summary>
        Task UpdateUserContextAsync(
            Guid userId,
            string entityType,
            Guid entityId,
            Dictionary<string, string> properties = null);
    }
}
