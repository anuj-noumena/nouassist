using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for user productivity profile information
    /// </summary>
    public class UserProductivityProfileDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Current cognitive load level
        /// </summary>
        public string CognitiveLoadLevel { get; set; }
        
        /// <summary>
        /// Current cognitive load value (0-100)
        /// </summary>
        public double CognitiveLoadValue { get; set; }
        
        /// <summary>
        /// When cognitive load was last updated
        /// </summary>
        public DateTime CognitiveLoadLastUpdated { get; set; }
        
        /// <summary>
        /// Work pattern description
        /// </summary>
        public string WorkPatternDescription { get; set; }
        
        /// <summary>
        /// Work pattern confidence
        /// </summary>
        public double? WorkPatternConfidence { get; set; }
        
        /// <summary>
        /// User preference settings
        /// </summary>
        public Dictionary<string, string> PreferenceSettings { get; set; }
        
        /// <summary>
        /// Recent performance data points
        /// </summary>
        public List<PerformanceDataPointDto> RecentPerformanceData { get; set; }
        
        /// <summary>
        /// Recent task completions
        /// </summary>
        public List<TaskCompletionRecordDto> RecentTaskCompletions { get; set; }
        
        /// <summary>
        /// External context references
        /// </summary>
        public List<ExternalContextRefDto> ExternalContextRefs { get; set; }
    }
}
