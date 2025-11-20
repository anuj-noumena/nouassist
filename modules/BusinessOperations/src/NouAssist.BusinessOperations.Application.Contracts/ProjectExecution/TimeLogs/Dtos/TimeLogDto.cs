using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for TimeLog entity
    /// </summary>
    public class TimeLogDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the user who logged time
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// ID of the project time was logged against
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Optional ID of the task time was logged against
        /// </summary>
        public Guid? TaskId { get; set; }
        
        /// <summary>
        /// Duration of time logged in hours
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        public DateTime LogDate { get; set; }
        
        /// <summary>
        /// Optional notes about the logged time
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Optional ID of the resource instance
        /// </summary>
        public Guid? ResourceInstanceId { get; set; }
        
        /// <summary>
        /// Name of the project (for display)
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Name of the task (for display)
        /// </summary>
        public string TaskName { get; set; }
        
        /// <summary>
        /// Name of the user (for display)
        /// </summary>
        public string UserName { get; set; }
    }
}
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for TimeLog entity
    /// </summary>
    public class TimeLogDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the user who logged time
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// User's name (for display purposes)
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// ID of the project time was logged against
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Project name (for display purposes)
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Optional ID of the task time was logged against
        /// </summary>
        public Guid? TaskId { get; set; }
        
        /// <summary>
        /// Task name (for display purposes)
        /// </summary>
        public string TaskName { get; set; }
        
        /// <summary>
        /// Duration of time logged in hours
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        public DateTime LogDate { get; set; }
        
        /// <summary>
        /// Optional notes about the logged time
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Optional ID of the resource instance
        /// </summary>
        public Guid? ResourceInstanceId { get; set; }
    }
}
