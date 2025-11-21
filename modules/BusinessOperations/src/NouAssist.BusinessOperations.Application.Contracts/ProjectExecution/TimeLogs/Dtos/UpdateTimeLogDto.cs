using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for updating a time log
    /// </summary>
    public class UpdateTimeLogDto
    {
        /// <summary>
        /// Duration of time logged in hours
        /// </summary>
        [Required]
        [Range(0.1, 24)]
        public double Hours { get; set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        [Required]
        public DateTime LogDate { get; set; }
        
        /// <summary>
        /// Optional notes about the logged time
        /// </summary>
        [StringLength(ProjectExecutionConsts.MaxTimeLogNotesLength)]
        public string Notes { get; set; }
        
        /// <summary>
        /// Optional ID of the resource instance
        /// </summary>
        public Guid? ResourceInstanceId { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for updating an existing time log
    /// </summary>
    public class UpdateTimeLogDto
    {
        /// <summary>
        /// Optional ID of the task time was logged against
        /// </summary>
        public Guid? TaskId { get; set; }
        
        /// <summary>
        /// Duration of time logged in hours
        /// </summary>
        [Required]
        [System.ComponentModel.DataAnnotations.Range(0.1, 24)]
        public double Hours { get; set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        [Required]
        public DateTime LogDate { get; set; }
        
        /// <summary>
        /// Optional notes about the logged time
        /// </summary>
        [StringLength(ProjectExecutionConsts.MaxTimeLogNotesLength)]
        public string Notes { get; set; }
        
        /// <summary>
        /// Optional ID of the resource instance
        /// </summary>
        public Guid? ResourceInstanceId { get; set; }
    }
}
