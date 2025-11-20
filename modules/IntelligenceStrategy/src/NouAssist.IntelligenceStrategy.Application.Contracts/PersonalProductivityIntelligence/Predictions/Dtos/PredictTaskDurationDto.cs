using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for predicting task duration
    /// </summary>
    public class PredictTaskDurationDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Task ID
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }
        
        /// <summary>
        /// Task name
        /// </summary>
        [Required]
        [StringLength(PersonalProductivityIntelligenceConsts.MaxNameLength)]
        public string TaskName { get; set; }
        
        /// <summary>
        /// Task description
        /// </summary>
        [StringLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)]
        public string TaskDescription { get; set; }
        
        /// <summary>
        /// Task context
        /// </summary>
        public TaskContextDto TaskContext { get; set; }
    }
}
