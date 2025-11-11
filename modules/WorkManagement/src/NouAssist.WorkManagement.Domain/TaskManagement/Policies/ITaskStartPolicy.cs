using NouAssist.WorkManagement.TaskManagement.Aggregates;

namespace NouAssist.WorkManagement.TaskManagement.Policies;

public interface ITaskStartPolicy
{
    (bool CanStart, string FailureReason) Check(TaskInstance taskInstance);
}