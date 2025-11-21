using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ProjectExecution.Entities
{
    /// <summary>
    /// Represents a dependency between tasks
    /// </summary>
    public class TaskDependency : Entity
    {
        /// <summary>
        /// ID of the task that has the dependency
        /// </summary>
        public Guid TaskId { get; private set; }
        
        /// <summary>
        /// ID of the task that is depended on
        /// </summary>
        public Guid DependsOnTaskId { get; private set; }
        
        /// <summary>
        /// Type of dependency
        /// </summary>
        public DependencyType DependencyType { get; private set; }
        
        protected TaskDependency() { }
        
        internal TaskDependency(
            Guid taskId,
            Guid dependsOnTaskId,
            DependencyType dependencyType)
        {
            TaskId = taskId;
            DependsOnTaskId = dependsOnTaskId;
            DependencyType = dependencyType;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { TaskId, DependsOnTaskId, DependencyType };
        }
    }
}
