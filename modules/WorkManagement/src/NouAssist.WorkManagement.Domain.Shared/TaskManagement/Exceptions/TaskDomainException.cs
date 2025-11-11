using System;
using Volo.Abp;

namespace NouAssist.WorkManagement.TaskManagement.Exceptions
{
    public class TaskDomainException : BusinessException
    {
        public TaskDomainException(string code, string message = null) 
            : base(code, message)
        {
        }
    }

    public class TaskNotFoundException : TaskDomainException
    {
        public TaskNotFoundException(Guid taskId) 
            : base("Tasks:NotFound", $"Task with ID {taskId} was not found.")
        {
        }
    }

    public class TaskInstanceNotFoundException : TaskDomainException
    {
        public TaskInstanceNotFoundException(Guid taskInstanceId) 
            : base("Tasks:InstanceNotFound", $"Task instance with ID {taskInstanceId} was not found.")
        {
        }
    }

    public class TaskInvalidStateException : TaskDomainException
    {
        public TaskInvalidStateException(string message) 
            : base("Tasks:InvalidState", message)
        {
        }
    }

    public class TaskDelegationException : TaskDomainException
    {
        public TaskDelegationException(string message) 
            : base("Tasks:DelegationError", message)
        {
        }
    }

    public class TaskDependencyException : TaskDomainException
    {
        public TaskDependencyException(string message) 
            : base("Tasks:DependencyError", message)
        {
        }
    }
}