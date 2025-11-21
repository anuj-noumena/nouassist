using System;

namespace NouAssist.BusinessOperations.ProjectExecution.Exceptions
{
    /// <summary>
    /// Exception thrown when a project is not found
    /// </summary>
    public class ProjectNotFoundException : ProjectExecutionDomainException
    {
        /// <summary>
        /// ID of the project that wasn't found
        /// </summary>
        public Guid ProjectId { get; }

        /// <summary>
        /// Creates a new ProjectNotFoundException
        /// </summary>
        /// <param name="projectId">ID of the project that wasn't found</param>
        public ProjectNotFoundException(Guid projectId)
            : base("ProjectExecution:ErrorMessage:ProjectNotFound")
        {
            ProjectId = projectId;
            WithData("projectId", projectId);
        }
    }
}
