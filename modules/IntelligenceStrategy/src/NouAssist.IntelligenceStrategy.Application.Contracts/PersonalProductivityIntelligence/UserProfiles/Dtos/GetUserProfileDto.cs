using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for getting a user profile
    /// </summary>
    public class GetUserProfileDto
    {
        /// <summary>
        /// User ID to get profile for
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Whether to include recent performance data
        /// </summary>
        public bool IncludePerformanceData { get; set; }
        
        /// <summary>
        /// Whether to include recent task completions
        /// </summary>
        public bool IncludeTaskCompletions { get; set; }
        
        /// <summary>
        /// Maximum number of performance data points to include
        /// </summary>
        public int MaxPerformanceDataCount { get; set; } = 10;
        
        /// <summary>
        /// Maximum number of task completions to include
        /// </summary>
        public int MaxTaskCompletionsCount { get; set; } = 10;
    }
}
