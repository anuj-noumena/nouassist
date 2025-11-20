using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for task completion record
    /// </summary>
    public class TaskCompletionRecordDto
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the task
        /// </summary>
        public string TaskName { get; set; }
        
        /// <summary>
        /// Estimated duration in minutes
        /// </summary>
        public int EstimatedDuration { get; set; }
        
        /// <summary>
        /// Actual duration in minutes
        /// </summary>
        public int ActualDuration { get; set; }
        
        /// <summary>
        /// When the task was completed
        /// </summary>
        public DateTime CompletedAt { get; set; }
        
        /// <summary>
        /// Task context information
        /// </summary>
        public TaskContextDto TaskContext { get; set; }
        
        /// <summary>
        /// Estimation accuracy (percentage)
        /// </summary>
        public double EstimationAccuracy { get; set; }
    }
}
