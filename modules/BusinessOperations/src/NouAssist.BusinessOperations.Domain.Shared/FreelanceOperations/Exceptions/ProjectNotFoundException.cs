using System;

namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Exception thrown when a project is not found
    /// </summary>
    public class ProjectNotFoundException : FreelanceOperationsDomainException
    {
        public ProjectNotFoundException(Guid id)
            : base("FreelanceOperations:ErrorMessage:ProjectNotFound")
        {
            WithData("id", id);
        }
    }
}
