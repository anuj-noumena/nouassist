using System;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for TaskDependency entity
    /// </summary>
    public class TaskDependencyDto : EntityDto
    {
        /// <summary>
        /// ID of the task that has the dependency
        /// </summary>
        public Guid TaskId { get; set; }
        
        /// <summary>
        /// ID of the task that is depended on
        /// </summary>
        public Guid DependsOnTaskId { get; set; }
        
        /// <summary>
        /// Type of dependency
        /// </summary>
        public DependencyType DependencyType { get; set; }
        
        /// <summary>
        /// Name of the task that is depended on (for display)
        /// </summary>
        public string DependsOnTaskName { get; set; }
    }
}
using System;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos
{
    /// <summary>
    /// DTO for TaskDependency entity
    /// </summary>
    public class TaskDependencyDto : EntityDto
    {
        /// <summary>
        /// ID of the task that has the dependency
        /// </summary>
        public Guid TaskId { get; set; }
        
        /// <summary>
        /// ID of the task that is depended on
        /// </summary>
        public Guid DependsOnTaskId { get; set; }
        
        /// <summary>
        /// Name of the task that is depended on (for display purposes)
        /// </summary>
        public string DependsOnTaskName { get; set; }
        
        /// <summary>
        /// Type of dependency
        /// </summary>
        public DependencyType DependencyType { get; set; }
        
        /// <summary>
        /// String representation of the dependency type
        /// </summary>
        public string DependencyTypeString => DependencyType.ToString();
    }
}
