using System;

namespace NouAssist.BusinessOperations.ProjectExecution.Exceptions
{
    /// <summary>
    /// Exception thrown when a task is not found
    /// </summary>
    public class TaskNotFoundException : ProjectExecutionDomainException
    {
        /// <summary>
        /// ID of the task that wasn't found
        /// </summary>
        public Guid TaskId { get; }

        /// <summary>
        /// Creates a new TaskNotFoundException
        /// </summary>
        /// <param name="taskId">ID of the task that wasn't found</param>
        public TaskNotFoundException(Guid taskId)
            : base("ProjectExecution:ErrorMessage:TaskNotFound")
        {
            TaskId = taskId;
            WithData("taskId", taskId);
        }
    }
}
