using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for recording a task completion
    /// </summary>
    public class RecordTaskCompletionDto
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
        /// Estimated duration in minutes
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int EstimatedDuration { get; set; }
        
        /// <summary>
        /// Actual duration in minutes
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ActualDuration { get; set; }
        
        /// <summary>
        /// Task context
        /// </summary>
        public TaskContextDto TaskContext { get; set; }
    }
}
